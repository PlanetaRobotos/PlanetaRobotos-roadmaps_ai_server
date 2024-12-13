using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Users.UserRoadmaps.Delete;

public class UserRoadmapDeleteRequest : IRequestModel
{
    public Guid Id { get; init; }
}
