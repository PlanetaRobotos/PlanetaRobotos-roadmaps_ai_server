using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Core.Security;
using CourseAI.Domain.Entities.Identity;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OneOf;

namespace CourseAI.Application.Features.Purchases;

public class BuyPlanHandler(
    IUserService userService,
    IRoleService roleService,
    ILogger<IHandler<BuyPlanRequest>> logger)
    : IHandler<BuyPlanRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(BuyPlanRequest request, CancellationToken ct)
    {
        // Validate request
        var roles = new[] { Roles.Standard, Roles.Enterprise, Roles.User };
        
        logger.LogInformation($"Buying plan: {request.Plan}, roles: {string.Join(", ", roles)}");

        if (!roles.Contains(request.Plan))
            return Error.ServerError("Invalid plan selected.");

        var userResult = await userService.GetUser();
        var user = userResult.Match(
            user => user,
            error => throw new Exception(error.Message)
        );

        // var roles = await userManager.GetRolesAsync(user);
        // if (roles.Contains(request.Plan))
        // {
        //     return Error.ServerError("You already have this plan.");
        // }

        //TODO Handle payment processing here
        // ...

        // Assign the selected role
        var convertedUserId = Convert.ToInt64(user.Id);
        var assignResult = await roleService.AssignRoleAsync(convertedUserId, request.Plan);
        if (!assignResult)
            return Error.ServerError("Failed to assign role.");

        // Optionally, remove the 'User' role if roles are exclusive
        // await roleService.RemoveRoleAsync(convertedUserId, Roles.User);

        return Unit.Value;
    }
}