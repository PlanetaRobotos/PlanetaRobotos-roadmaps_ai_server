using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Context;
using Mediator;
using OneOf;

namespace CourseAI.Application.Features.Users.Update;

public class UserUpdateHandler(AppDbContext dbContext) : IHandler<UserUpdateRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(UserUpdateRequest command, CancellationToken ct)
    {
        var user = await dbContext.Users.FindAsync([command.Id,], ct);

        // var user = command.MapToEntity();

        return Unit.Value;
    }
}
