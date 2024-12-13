using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Domain.Context;
using Fleet.Domain.Entities;
using Mediator;
using OneOf;

namespace Fleet.Application.Features.Users.UserRoadmaps.Delete;

public class ClientDeleteHandler(AppDbContext dbContext) : IHandler<UserRoadmapDeleteRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(UserRoadmapDeleteRequest request, CancellationToken ct)
    {
        var UserRoadmap = await dbContext.UserRoadmaps.FindAsync([request.Id,], ct);

        if (UserRoadmap is null)
        {
            return Error.NotFound<UserRoadmap>();
        }

        dbContext.UserRoadmaps.Remove(UserRoadmap);
        await dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
