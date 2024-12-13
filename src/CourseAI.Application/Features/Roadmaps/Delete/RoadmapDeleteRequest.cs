using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Roadmaps.Delete;

public class RoadmapDeleteRequest : IRequestModel
{
    public Guid Id { get; init; }
}
