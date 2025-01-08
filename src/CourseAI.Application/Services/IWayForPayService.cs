using CourseAI.Application.Models.WayForPays;

namespace CourseAI.Infrastructure.Services;

public interface IWayForPayService
{
    string GenerateSignature(WayForPayRequest request);
    bool VerifyCallback(WayForPayResponse response);
}