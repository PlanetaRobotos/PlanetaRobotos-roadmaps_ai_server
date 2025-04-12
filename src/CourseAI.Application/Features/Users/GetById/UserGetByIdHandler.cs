using CourseAI.Application.Core;
using CourseAI.Application.Extensions;
using CourseAI.Application.Models;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using OneOf;
using Error = CourseAI.Application.Models.Error;

namespace CourseAI.Application.Features.Users.GetById;

public class UserGetByIdHandler(AppDbContext dbContext, UserManager<User> userManager) : IHandler<UserGetByIdRequest, UserModel>
{
    public async ValueTask<OneOf<UserModel, Error>> Handle(UserGetByIdRequest request, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        
        if (user == null)
        {
            return Error.NotFound<User>();
        }

        return user.Adapt<UserModel>();
    }
}
