using CourseAI.Domain.Entities;

namespace CourseAI.Application.Services;

public interface IEmailVerificationLinkFactory
{
    string? Create(EmailVerificationToken emailVerificationToken, string returnUrl);
}