using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Domain.Context;
using Mediator;
using OneOf;

namespace Fleet.Application.Features.Users.Update;

public class UserUpdateHandler(AppDbContext dbContext) : IHandler<UserUpdateRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(UserUpdateRequest command, CancellationToken ct)
    {
        var user = await dbContext.Users.FindAsync([command.Id,], ct);

        // var user = command.MapToEntity();

        return Unit.Value;
    }
}
