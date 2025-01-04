using System.Security.Claims;
using CourseAI.Application.Core;
using CourseAI.Application.Features.Tokens;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Core.Enums;
using CourseAI.Core.Security;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using CourseAI.Domain.Entities.Transactions;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Purchases;

public class BuyPlanHandler(
    IUserService userService,
    IRoleService roleService)
    : IHandler<BuyPlanRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(BuyPlanRequest request, CancellationToken ct)
    {
        // Validate request
        if (!new[] { Roles.Standard, Roles.Enterprise }.Contains(request.Plan))
            return Error.ServerError("Invalid plan selected.");
        
        var userResult = await userService.GetUser();
        var user = userResult.Match(
            user => user,
            error => throw new Exception(error.Message)
        );
        
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