using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Entities.Identity;
using Mediator;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Users.Delete;

public class UserDeleteHandler(UserManager<User> userManager): IHandler<UserDeleteRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(UserDeleteRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return Error.NotFound<User>();
        }

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        { 
            return Error.ServerError(result.Errors.ToString());
        }

        return Unit.Value;
    }
}