using Fleet.Application.Core;

namespace Fleet.Application.Features.Lessons.Delete;

public class LessonDeleteRequest : IRequestModel
{
    public Guid Id { get; init; }
}
