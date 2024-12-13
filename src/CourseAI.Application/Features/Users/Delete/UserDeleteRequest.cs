using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Users.Delete;

public class UserDeleteRequest : IRequestModel
{
    public long Id { get; init; }
}
