using CourseAI.Application.Models.AppSumoLicense;

namespace CourseAI.Application.Services;

public interface IAppSumoService
{
    Task<AppSumoTokenResponse> GetAccessTokenAsync(string code);
    Task<AppSumoLicenseResponse> GetLicenseKeyAsync(string accessToken);
    Task<AppSumoTokenResponse> RefreshTokenAsync(string refreshToken);
}