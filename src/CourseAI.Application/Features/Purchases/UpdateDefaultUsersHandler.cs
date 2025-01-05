using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Core.Security;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Purchases;

public class UpdateDefaultUsersHandler(AppDbContext dbContext, IRoleService roleService, UserManager<User> userManager)
    : IHandler<UpdateDefaultUsersRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(UpdateDefaultUsersRequest request, CancellationToken ct)
    {
        // Validate request
        if (!new[] { Roles.User }.Contains(request.Plan))
            return Error.ServerError("Invalid plan selected.");

        var users = await dbContext.Users.ToListAsync(cancellationToken: ct);

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);

            if (roles.Count > 0)
                continue;

            var assignResult = await roleService.AssignRoleAsync(user.Id, request.Plan);
            if (!assignResult)
                return Error.ServerError("Failed to assign role.");
        }

        return Unit.Value;
    }
}