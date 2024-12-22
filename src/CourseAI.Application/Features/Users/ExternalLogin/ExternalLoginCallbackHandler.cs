using System.Security.Claims;
using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Users.ExternalLogin
{
    public class ExternalLoginCallbackHandler(IHttpContextAccessor accessor, UserManager<User> userManager, IJwtProvider jwtProvider) : IHandler<ExternalLoginCallbackRequest, string>
    {

        public async ValueTask<OneOf<string, Error>> Handle(ExternalLoginCallbackRequest request, CancellationToken ct)
        {
            try
            {
                var authenticateResult = await accessor.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

                if (!authenticateResult.Succeeded)
                {
                    Error.ServerError($"Failed to authenticate user {authenticateResult.Failure.Message}");
                }

                var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
                var fullName = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;
                var profilePicture = authenticateResult.Principal.FindFirst("picture")?.Value;

                if (email == null)
                    Error.ServerError($"Email claim not found.");

                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    // Create a new user
                    user = new User
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        // NormalizedUserName = fullName
                    };
                    var identityResult = await userManager.CreateAsync(user);
                    if (!identityResult.Succeeded)
                    {
                        Error.ServerError($"{identityResult.Errors}");
                    }
                }

                var token = jwtProvider.Create(user);

                return token;
            }
            catch (Exception e)
            {
                Error.ServerError($"{e.Message}");
                //TODO redirect to error page
                return "";
            }
        }
    }
}
