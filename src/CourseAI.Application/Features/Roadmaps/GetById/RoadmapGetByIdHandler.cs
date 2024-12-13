using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using CourseAI.Domain.Entities.Roadmaps;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Roadmaps.GetById;

public class RoadmapGetByIdHandler(AppDbContext dbContext) : IHandler<RoadmapGetByIdRequest, RoadmapModel>
{
    public async ValueTask<OneOf<RoadmapModel, Error>> Handle(RoadmapGetByIdRequest request, CancellationToken ct)
    {
        var Roadmap = await dbContext.Roadmaps
            .Include(e => e.Modules)
            .ThenInclude(m => m.Lessons)
            .Where(e => e.Id == request.Id)
            .FirstOrDefaultAsync(ct);

        if (Roadmap is null)
        {
            return Error.NotFound<Roadmap>();
        }

        return Roadmap.Adapt<RoadmapModel>();
    }
}
