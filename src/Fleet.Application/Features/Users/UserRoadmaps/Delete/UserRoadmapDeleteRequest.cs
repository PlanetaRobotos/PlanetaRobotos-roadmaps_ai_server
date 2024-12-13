using Fleet.Application.Core;

namespace Fleet.Application.Features.Users.UserRoadmaps.Delete;

public class UserRoadmapDeleteRequest : IRequestModel
{
    public Guid Id { get; init; }
}
