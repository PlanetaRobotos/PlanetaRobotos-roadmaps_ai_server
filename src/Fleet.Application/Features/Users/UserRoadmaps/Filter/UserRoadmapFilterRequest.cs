using Fleet.Application.Core;
using Fleet.Application.Models.Shared;
using Fleet.Application.Models.UserRoadmaps;

namespace Fleet.Application.Features.Users.UserRoadmaps.Filter;

public class UserRoadmapFilterRequest : FilterRequestBase<UserRoadmapFilterRequest>, IRequestModel<Filtered<UserRoadmapModel>>
{
    public long UserId { get; init; }
}
