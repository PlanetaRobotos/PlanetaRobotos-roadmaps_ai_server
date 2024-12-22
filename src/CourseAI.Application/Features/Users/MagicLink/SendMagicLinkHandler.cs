using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Domain.Entities.Identity;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace CourseAI.Application.Features.Users.MagicLink
{
    public class SendMagicLinkHandler(UserManager<User> userManager, IMailgunService mailgunService, IUrlHelper urlHelper)
        : IHandler<SendMagicLinkRequest>
    {
        public async ValueTask<OneOf<Unit, Error>> Handle(SendMagicLinkRequest request, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return Error.NotFound<User>();
            }

            var token = await userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "MagicLink");

            var magicLink = urlHelper.Action("MagicLinkLogin",
                "Users",
                new { userId = user.Id, token, },
                "https");

            var emailBody = $"Click <a href='{magicLink}'>here</a> to log in to your account.";

            try
            {
                await mailgunService.SendEmailAsync(user.Email, "Your Magic Login Link", emailBody);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                // Log exception as needed
                return Error.ServerError("Failed to send magic link email.");
            }
        }
    }
}
