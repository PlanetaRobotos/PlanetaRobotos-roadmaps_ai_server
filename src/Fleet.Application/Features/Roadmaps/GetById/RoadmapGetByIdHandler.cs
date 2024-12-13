using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Application.Models.Roadmaps;
using Fleet.Domain.Context;
using Fleet.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Fleet.Application.Features.Roadmaps.GetById;

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
