using System.Security.Claims;
using System.Text.RegularExpressions;
using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Users.ExternalLogin;

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
            var name = authenticateResult.Principal.FindFirst(ClaimTypes.GivenName)?.Value;
            var profilePicture = authenticateResult.Principal.FindFirst("picture")?.Value;
                
            if (string.IsNullOrEmpty(name) || !Regex.IsMatch(name, @"^[a-zA-Z0-9]+$")) 
                name = Regex.Replace(name ?? string.Empty, @"[^a-zA-Z0-9]", string.Empty);

            if (email == null)
                Error.ServerError($"Email claim not found. {authenticateResult.Principal.Claims}");

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Error.ServerError($"User not found {email}");

                user = new User
                {
                    UserName = name,
                    Email = email,
                    EmailConfirmed = true
                };
                var identityResult = await userManager.CreateAsync(user);
                if (!identityResult.Succeeded)
                {
                    Error.ServerError($"identityResult Failed: {identityResult.Errors}");
                }
            }

            var token = jwtProvider.Create(user);

            return token;
        }
        catch (Exception e)
        {
            return Error.ServerError($"ExternalLoginCallback failed: {e.Message}");
        }
    }
}