using System.Security.Cryptography;
using System.Text;
using CourseAI.Application.Models.AppSumoLicense;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.AppSumo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NeerCore.DependencyInjection;

namespace CourseAI.Infrastructure.Services;

[Service]
public class AppSumoWebhookService : IAppSumoWebhookService
{
    private readonly ILogger<AppSumoWebhookService> _logger;
    private readonly AppDbContext _context;
    private readonly IRoleService _roleService;
    private readonly string _apiKey;

    public AppSumoWebhookService(
        ILogger<AppSumoWebhookService> logger,
        AppDbContext context,
        IRoleService roleService,
        IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _roleService = roleService;
        _apiKey = configuration["AppSumo:ApiKey"];
    }

    public bool VerifyWebhookSignature(string body, string timestamp, string signature)
    {
        var message = $"{timestamp}{body}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_apiKey));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
        var computedSignature = BitConverter.ToString(computedHash).Replace("-", "").ToLower();
        
        return computedSignature == signature;
    }

    public async Task ProcessWebhookEvent(AppSumoWebhookRequest request)
    {
        try
        {
            switch (request.@event.ToLower())
            {
                case "purchase":
                    await HandlePurchaseEvent(request);
                    break;
                case "activate":
                    await HandleActivateEvent(request);
                    break;
                case "upgrade":
                    await HandleUpgradeEvent(request);
                    break;
                case "downgrade":
                    await HandleDowngradeEvent(request);
                    break;
                case "deactivate":
                    await HandleDeactivateEvent(request);
                    break;
                default:
                    _logger.LogWarning("Unknown webhook event type: {Event}", request.@event);
                    throw new ArgumentException($"Unknown webhook event type: {request.@event}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ProcessWebhookEvent: {Event}", request.@event);
            throw; // Rethrow to be caught by controller
        }
    }
    
    private async Task HandlePurchaseEvent(AppSumoWebhookRequest request)
    {
        // Create initial license record when purchase happens
        var license = new AppSumoLicense
        {
            LicenseKey = request.license_key,
            PlanId = request.plan_id,
            Status = request.license_status,
            Created = DateTime.UtcNow
            // UserId will be null at this point - that's OK!
        };

        _context.AppSumoLicenses.Add(license);
        await _context.SaveChangesAsync();
    }

    private async Task HandleActivateEvent(AppSumoWebhookRequest request)
    {
        var license = await _context.AppSumoLicenses
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.LicenseKey == request.license_key);
        
        _logger.LogInformation($"Activating license: {request.license_key}. Tier: {request.tier}");

        if (license is not null)
        {
            license.Status = "active";
            license.Tier = request.tier ?? 1;
            await _context.SaveChangesAsync();
        }
    }

    private async Task HandleUpgradeEvent(AppSumoWebhookRequest request)
    {
        if (string.IsNullOrEmpty(request.prev_license_key))
            return;

        var oldLicense = await _context.AppSumoLicenses
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.LicenseKey == request.prev_license_key);

        if (oldLicense?.User != null)
        {
            // Create new license
            var newLicense = new AppSumoLicense
            {
                LicenseKey = request.license_key,
                UserId = oldLicense.UserId,
                PlanId = request.plan_id,
                Status = "active",
                Created = DateTime.UtcNow
            };

            _context.AppSumoLicenses.Add(newLicense);
            oldLicense.Status = "deactivated";

            // Update user role
            var newRole = GetRoleNameForTier(request.tier ?? 1);
            await _roleService.AssignRoleAsync(oldLicense.User.Id, newRole);

            await _context.SaveChangesAsync();
        }
    }

    private async Task HandleDowngradeEvent(AppSumoWebhookRequest request)
    {
        // Similar to upgrade, but with downgraded tier
        await HandleUpgradeEvent(request);
    }

    private async Task HandleDeactivateEvent(AppSumoWebhookRequest request)
    {
        var license = await _context.AppSumoLicenses
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.LicenseKey == request.license_key);

        if (license is not null)
        {
            license.Status = "deactivated";
            await _roleService.RemoveRoleAsync(license.User.Id, GetRoleNameForTier(request.tier ?? 1));
            await _context.SaveChangesAsync();
        }
    }

    private string GetRoleNameForTier(int tier)
    {
        return tier switch
        {
            1 => "AppSumo_Creator",
            2 => "AppSumo_Studio",
            _ => throw new ArgumentException($"Invalid tier: {tier}")
        };
    }
}