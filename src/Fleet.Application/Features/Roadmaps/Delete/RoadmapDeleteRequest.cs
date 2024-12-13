using Fleet.Application.Core;

namespace Fleet.Application.Features.Roadmaps.Delete;

public class RoadmapDeleteRequest : IRequestModel
{
    public Guid Id { get; init; }
}
