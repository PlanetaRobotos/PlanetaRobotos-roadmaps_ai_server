using Fleet.Application.Core;
using Fleet.Application.Models.Roadmaps;
using Fleet.Application.Models.Shared;

namespace Fleet.Application.Features.Roadmaps.Filter;

public class RoadmapFilterRequest : FilterRequestBase<RoadmapFilterRequest>, IRequestModel<Filtered<RoadmapModel>>;
