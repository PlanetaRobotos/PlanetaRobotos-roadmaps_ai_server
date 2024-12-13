using CourseAI.Application.Core;
using CourseAI.Application.Models.Shared;
using CourseAI.Application.Models.UserRoadmaps;

namespace CourseAI.Application.Features.Users.UserRoadmaps.Filter;

public class UserRoadmapFilterRequest : FilterRequestBase<UserRoadmapFilterRequest>, IRequestModel<Filtered<UserRoadmapModel>>
{
    public long UserId { get; init; }
}
