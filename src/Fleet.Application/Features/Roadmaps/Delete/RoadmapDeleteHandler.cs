using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Domain.Context;
using Fleet.Domain.Entities;
using Mediator;
using OneOf;

namespace Fleet.Application.Features.Roadmaps.Delete;

public class ClientDeleteHandler(AppDbContext dbContext) : IHandler<RoadmapDeleteRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(RoadmapDeleteRequest request, CancellationToken ct)
    {
        var Roadmap = await dbContext.Roadmaps.FindAsync([request.Id,], ct);

        if (Roadmap is null)
        {
            return Error.NotFound<Roadmap>();
        }

        dbContext.Roadmaps.Remove(Roadmap);
        await dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
