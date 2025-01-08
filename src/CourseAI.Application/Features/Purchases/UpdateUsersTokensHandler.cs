using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Purchases;

public class UpdateUsersTokensHandler(AppDbContext dbContext, IRoleService roleService, UserManager<User> userManager)
    : IHandler<UpdateUsersTokensRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(UpdateUsersTokensRequest request, CancellationToken ct)
    {
        var users = await dbContext.Users.ToListAsync(cancellationToken: ct);

        foreach (var user in users)
        {
            user.Tokens += request.TokensAmount;
            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    return Error.ServerError($"updateResult Failed: {error.Description}");
            }
        }

        return Unit.Value;
    }
}