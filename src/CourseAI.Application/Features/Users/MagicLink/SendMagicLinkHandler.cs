using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using FluentEmail.Core;
using Mediator;
using Microsoft.Extensions.Logging;
using OneOf;

namespace CourseAI.Application.Features.Users.MagicLink
{
    public class SendMagicLinkHandler(
        ILogger<SendMagicLinkHandler> logger,
        IFluentEmail fluentEmail,
        AppDbContext dbContext,
        IEmailVerificationLinkFactory emailVerificationLinkFactory)
        : IHandler<SendMagicLinkRequest, string>
    {
        public async ValueTask<OneOf<string, Error>> Handle(SendMagicLinkRequest request, CancellationToken ct)
        {
            var user = await dbContext.Users.FindAsync([request.UserId], ct);
            
            if (user == null)
            {
                return Error.NotFound($"User not found {request.UserId}");
            }
            
            var utcNow = DateTime.UtcNow;
            var verificationToken = new EmailVerificationToken
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                CreatedOnUtc = utcNow,
                ExpiresOnUtc = utcNow.AddHours(24)
            };

            dbContext.EmailVerificationTokens.Add(verificationToken);
            await dbContext.SaveChangesAsync(ct);

            var verificationLink = emailVerificationLinkFactory.Create(verificationToken);

            if (verificationLink == null)
            {
                return Error.ServerError($"Failed to generate verification link for user {request.UserId}");
            }

            await fluentEmail
                .To(user.Email)
                .Subject("Email Verification to Levenue Courses")
                .Body($"Please verify your email by clicking <a href={verificationLink}>this link</a>", isHtml: true)
                .SendAsync();
            
            return verificationLink;
        }
    }
}
