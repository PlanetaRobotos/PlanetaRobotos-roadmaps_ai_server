using System.Security.Claims;
using System.Text.RegularExpressions;
using CourseAI.Application.Core;
using CourseAI.Application.Extensions;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Core.Security;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Users.ExternalLogin;

public class ExternalLoginCallbackHandler(
    IHttpContextAccessor accessor,
    UserManager<User> userManager,
    IJwtProvider jwtProvider,
    IRoleService roleService,
    IUserService userService
) : IHandler<ExternalLoginCallbackRequest, string>
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

            if (email == null)
                Error.ServerError($"Email claim not found. {authenticateResult.Principal.Claims}");

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = await userService.CreateUser(email, true, Roles.User.ToString());
                if (user == null)
                    return Error.ServerError("Failed to create user.");
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