using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Core.Security;
using Mediator;
using OneOf;

namespace CourseAI.Application.Features.Purchases;

public class DeletePlanHandler(
    IUserService userService,
    IRoleService roleService)
    : IHandler<DeletePlanRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(DeletePlanRequest request, CancellationToken ct)
    {
        // Validate request
        if (!Enum.IsDefined(typeof(Roles), request.Plan))
            return Error.ServerError("Invalid plan selected.");
        
        var userResult = await userService.GetUser();
        var user = userResult.Match(
            user => user,
            error => throw new Exception(error.Message)
        );

        var convertedUserId = Convert.ToInt64(user.Id);
        var result = await roleService.RemoveRoleAsync(convertedUserId, request.Plan);
        if (!result)
            return Error.ServerError("Failed to assign role.");

        return Unit.Value;
    }
}