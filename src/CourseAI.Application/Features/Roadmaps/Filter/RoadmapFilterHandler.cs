using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Models.Shared;
using CourseAI.Domain.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Roadmaps.Filter;

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
