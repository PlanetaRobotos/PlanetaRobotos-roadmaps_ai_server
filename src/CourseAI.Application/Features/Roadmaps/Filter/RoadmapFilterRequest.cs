using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Models.Shared;

namespace CourseAI.Application.Features.Roadmaps.Filter;

public class RoadmapFilterRequest : FilterRequestBase<RoadmapFilterRequest>, IRequestModel<Filtered<RoadmapModel>>;
