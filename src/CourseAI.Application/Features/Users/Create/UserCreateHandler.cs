using CourseAI.Application.Core;
using CourseAI.Application.Extensions;
using CourseAI.Application.Models;
using CourseAI.Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Users.Create;

public class UserCreateHandler(
    UserManager<User> userManager
    ) : IHandler<UserCreateRequest, UserModel>
{
    public async ValueTask<OneOf<UserModel, Error>> Handle(UserCreateRequest request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user != null)
        {
            throw new Exception("User already exists");
        }
        
        user = new User
        {
            Email = request.Email,
            UserName = request.Email.ToUsername(),
            EmailConfirmed = false
        };

        var identityResult = await userManager.CreateAsync(user);
        if (!identityResult.Succeeded)
        {
            return Error.ServerError($"identityResult Failed: {identityResult.Errors}");
        }

        return user.Adapt<UserModel>();
    }
}