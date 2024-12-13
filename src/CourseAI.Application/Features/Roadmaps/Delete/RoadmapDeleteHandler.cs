using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using CourseAI.Domain.Entities.Roadmaps;
using Mediator;
using OneOf;

namespace CourseAI.Application.Features.Roadmaps.Delete;

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
