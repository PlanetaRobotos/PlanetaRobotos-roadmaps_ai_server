using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using Mediator;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Users.Update;

public class UserUpdateHandler(AppDbContext dbContext, UserManager<User> userManager) : IHandler<UserUpdateRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(UserUpdateRequest request, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return Error.NotFound<User>();
        }

        user.UserName = request.UserName;
        user.Email = request.Email;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return Error.ServerError(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        
        return Unit.Value;
    }
}
