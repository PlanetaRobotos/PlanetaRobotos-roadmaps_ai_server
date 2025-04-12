using System.Security.Cryptography;
using System.Text;
using CourseAI.Api.Core;
using CourseAI.Application.Models.AppSumoLicense;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using CourseAI.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseAI.Api.Controllers;

public class LicenseController(
    UserManager<User> userManager,
    IAppSumoService appSumoService,
    IConfiguration configuration,
    IRoleService roleService,
    IAppSumoWebhookService webhookService,
    AppDbContext dbContext) : V1Controller
{
    [HttpGet("oauth/redirect")]
    public async Task<IActionResult> HandleOAuthRedirect([FromQuery] string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            Logger.LogInformation("Initial validation request from AppSumo");
            return Ok();
        }

        try
        {
            // Exchange code for access token
            var tokenResponse = await appSumoService.GetAccessTokenAsync(code);

            if (!string.IsNullOrEmpty(tokenResponse.error))
            {
                return BadRequest(tokenResponse.error);
            }

            // Get license key using access token
            AppSumoLicenseResponse licenseResponse;
            var finalTokens = tokenResponse; // Track the most recent valid tokens

            try
            {
                licenseResponse = await appSumoService.GetLicenseKeyAsync(tokenResponse.access_token);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Handle 401 by refreshing the token
                var refreshResponse = await appSumoService.RefreshTokenAsync(tokenResponse.refresh_token);
                if (!string.IsNullOrEmpty(refreshResponse.error))
                {
                    return BadRequest("Failed to refresh token");
                }

                // Retry with new access token
                licenseResponse = await appSumoService.GetLicenseKeyAsync(refreshResponse.access_token);
                finalTokens = refreshResponse; // Update to use refresh tokens
            }

            // Find the existing license
            var license = await dbContext.AppSumoLicenses
                .FirstOrDefaultAsync(l => l.LicenseKey == licenseResponse.license_key);

            if (license == null)
            {
                return BadRequest("License not found");
            }

            // Update the license with the most recent valid tokens
            license.AccessToken = finalTokens.access_token;
            license.RefreshToken = finalTokens.refresh_token;
            license.TokenExpiry = DateTime.UtcNow.AddSeconds(finalTokens.expires_in);
            await dbContext.SaveChangesAsync();

            // Redirect to frontend registration page
            var clientUrl = configuration["Client:Url"];
            return Redirect($"{clientUrl}/signin?key={licenseResponse.license_key}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("complete-registration")]
    public async Task<IActionResult> CompleteRegistration([FromBody] CompleteRegistrationRequest request)
    {
        var license = await dbContext.AppSumoLicenses
            .FirstOrDefaultAsync(l => l.LicenseKey == request.LicenseKey);

        if (license == null)
        {
            return BadRequest("License not found");
        }

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            Logger.LogInformation("User with email {Email} not found", request.Email);
            return NotFound("User not found");
        }

        // Link user to license
        license.UserId = user.Id;
        await dbContext.SaveChangesAsync();

        // Add role
        var role = MapPlanToRole(license.Tier);
        await roleService.AssignRoleAsync(user.Id, role);
        Logger.LogInformation("Assigned role {Role} to user {Email}", role, request.Email);

        return Ok();
    }

    [HttpPost("appsumo/webhook")]
    public async Task<IActionResult> HandleWebhook([FromBody] AppSumoWebhookRequest request)
    {
        try
        {
            Logger.LogInformation("Received AppSumo webhook: {Event}", request.@event);

            if (request.test)
            {
                return Ok(new
                {
                    success = true,
                    event_type = request.@event,
                    message = "Test webhook received successfully"
                });
            }

            // Process the webhook event
            await webhookService.ProcessWebhookEvent(request);

            return Ok(new
            {
                success = true,
                event_type = request.@event,
                message = $"Processed {request.@event} event successfully"
            });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error processing webhook: {Event}", request?.@event);

            return StatusCode(500, new
            {
                success = false,
                event_type = request?.@event,
                message = "Internal server error processing webhook",
                error = ex.Message
            });
        }
    }

    private static string MapPlanToRole(int planId)
    {
        return planId switch
        {
            1 => "AppSumo_1",
            2 => "AppSumo_2",
            _ => throw new Exception("Invalid plan ID")
        };
    }
}