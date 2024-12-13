using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;

namespace CourseAI.Application.Features.Roadmaps.GetById;

public class RoadmapGetByIdRequest : IRequestModel<RoadmapModel>
{
    public Guid Id { get; init; }
}
