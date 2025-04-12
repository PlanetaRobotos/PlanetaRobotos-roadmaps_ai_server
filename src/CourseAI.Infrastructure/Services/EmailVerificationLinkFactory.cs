using CourseAI.Application.Features.Users.MagicLink;
using CourseAI.Application.Services;
using CourseAI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NeerCore.DependencyInjection;

namespace CourseAI.Infrastructure.Services;

[Service]
public class EmailVerificationLinkFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    : IEmailVerificationLinkFactory
{
    public string? Create(EmailVerificationToken emailVerificationToken, string returnUrl)
    {
        string? verificationLink = linkGenerator.GetUriByName(
            httpContextAccessor.HttpContext!,
            "VerifyEmail",
            new VerifyEmailRequest
            {
                Token = emailVerificationToken.Id,
                ReturnUrl = returnUrl
            }
        );

        return verificationLink ?? throw new Exception("Failed to generate verification link");
    }
}