using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Entities.Identity;
using Mediator;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Users.MagicLink
{
    public class MagicLinkLoginHandler(UserManager<User> userManager)
        : IHandler<MagicLinkLoginRequest>
    {
        public async ValueTask<OneOf<Unit, Error>> Handle(MagicLinkLoginRequest request, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                return Error.NotFound<User>();
            }

            var isValid = await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "MagicLink", request.Token);

            if (!isValid)
            {
                return Error.ServerError("Invalid or expired magic link.");
            }

            // await signInManager.SignInAsync(user, isPersistent: false);
            return Unit.Value;
        }
    }
}
