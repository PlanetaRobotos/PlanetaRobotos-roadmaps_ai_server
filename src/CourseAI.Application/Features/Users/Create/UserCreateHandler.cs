using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Users.Create;

public class UserCreateHandler(AppDbContext dbContext, UserManager<User> userManager) : IHandler<UserCreateRequest, UserModel>
{
    public async ValueTask<OneOf<UserModel, Error>> Handle(UserCreateRequest request, CancellationToken ct)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            return Error.ServerError(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return user.Adapt<UserModel>();
    }
}
