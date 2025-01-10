using CourseAI.Application.Core;
using CourseAI.Application.Extensions;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Core.Security;
using CourseAI.Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Users.Create;

public class UserCreateHandler(
    UserManager<User> userManager,
    IUserService userService
) : IHandler<UserCreateRequest, UserModel>
{
    public async ValueTask<OneOf<UserModel, Error>> Handle(UserCreateRequest request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is { EmailConfirmed: true })
        {
            return user.Adapt<UserModel>();
        }

        user = await userService.CreateUser(request.Email, false, [Roles.user.ToString()]);
        if (user == null)
        {
            return Error.ServerError("Failed to create user.");
        }

        return user.Adapt<UserModel>();
    }
}