using System.Net.Http.Json;
using CourseAI.Application.Models.AppSumoLicense;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using Microsoft.Extensions.Configuration;
using NeerCore.DependencyInjection;

namespace CourseAI.Infrastructure.Services;

[Service]
public class AppSumoService(
    HttpClient httpClient,
    IConfiguration configuration,
    AppDbContext context) : IAppSumoService
{
    public async Task<AppSumoTokenResponse> GetAccessTokenAsync(string code)
    {
        var tokenEndpoint = "https://appsumo.com/openid/token/";
        
        var tokenRequest = new Dictionary<string, string>
        {
            ["client_id"] = configuration["AppSumo:ClientId"],
            ["client_secret"] = configuration["AppSumo:ClientSecret"],
            ["code"] = code,
            ["redirect_uri"] = configuration["AppSumo:RedirectUri"],
            ["grant_type"] = "authorization_code"
        };

        var response = await httpClient.PostAsJsonAsync(tokenEndpoint, tokenRequest);
        return await response.Content.ReadFromJsonAsync<AppSumoTokenResponse>();
    }

    public async Task<AppSumoLicenseResponse> GetLicenseKeyAsync(string accessToken)
    {
        var licenseEndpoint = $"https://appsumo.com/openid/license_key/?access_token={accessToken}";
        var response = await httpClient.GetAsync(licenseEndpoint);
        return await response.Content.ReadFromJsonAsync<AppSumoLicenseResponse>();
    }

    public async Task<AppSumoTokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var tokenEndpoint = "https://appsumo.com/openid/token/";
        
        var refreshRequest = new Dictionary<string, string>
        {
            ["client_id"] = configuration["AppSumo:ClientId"],
            ["client_secret"] = configuration["AppSumo:ClientSecret"],
            ["refresh_token"] = refreshToken,
            ["grant_type"] = "refresh_token"
        };

        var response = await httpClient.PostAsJsonAsync(tokenEndpoint, refreshRequest);
        return await response.Content.ReadFromJsonAsync<AppSumoTokenResponse>();
    }
}