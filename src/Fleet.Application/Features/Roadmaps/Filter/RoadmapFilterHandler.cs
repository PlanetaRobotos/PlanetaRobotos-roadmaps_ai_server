using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Application.Models.Roadmaps;
using Fleet.Application.Models.Shared;
using Fleet.Domain.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Fleet.Application.Features.Roadmaps.Filter;

public class RoadmapFilterHandler(AppDbContext dbContext) : IHandler<RoadmapFilterRequest, Filtered<RoadmapModel>>
{
    public async ValueTask<OneOf<Filtered<RoadmapModel>, Error>> Handle(RoadmapFilterRequest request, CancellationToken ct)
    {
        var roadmaps = await dbContext.Roadmaps.ToArrayAsync(ct);

        return new Filtered<RoadmapModel>
        {
            Data = roadmaps.Select(c => c.Adapt<RoadmapModel>()).ToArray(),
            Total = roadmaps.Length,
            Columns = null,
        };
    }
}
