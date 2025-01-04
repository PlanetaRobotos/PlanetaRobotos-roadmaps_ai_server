using CourseAI.Application.Core;
using CourseAI.Application.Services;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using OneOf;
using Error = CourseAI.Application.Models.Error;

namespace CourseAI.Application.Features.Users.MagicLink;

public class RedirectHandler(
    UserManager<User> userManager,
    IJwtProvider jwtProvider)
    : IHandler<RedirectRequest, string>
{
    public async ValueTask<OneOf<string, Error>> Handle(RedirectRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Error.Unauthorized("User not found");
        }
        
        if (user is { EmailConfirmed: false })
        {
            return Error.ServerError("User email not confirmed");
        }

        var jwtToken = jwtProvider.Create(user);
        
        return jwtToken;
    }
}