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
        var roadmaps = await dbContext.Roadmaps
            .Include(x => x.Modules)
            .ThenInclude(x => x.Lessons)
            .ThenInclude(x => x.Quizzes)
            .ToArrayAsync(ct);

        var roadmapModels = roadmaps.Select(c => c.Adapt<RoadmapModel>()).ToArray();
        
        return new Filtered<RoadmapModel>
        {
            Data = roadmapModels,
            Total = roadmaps.Length,
            Columns = null,
        };
    }
}
