using Fleet.Application.Core;
using Fleet.Application.Models.Roadmaps;

namespace Fleet.Application.Features.Roadmaps.GetById;

public class RoadmapGetByIdRequest : IRequestModel<RoadmapModel>
{
    public Guid Id { get; init; }
}
