using CourseAI.Application.Models.AppSumoLicense;

namespace CourseAI.Infrastructure.Services;

public interface IAppSumoWebhookService
{
    bool VerifyWebhookSignature(string body, string timestamp, string signature);
    Task ProcessWebhookEvent(AppSumoWebhookRequest request);
}