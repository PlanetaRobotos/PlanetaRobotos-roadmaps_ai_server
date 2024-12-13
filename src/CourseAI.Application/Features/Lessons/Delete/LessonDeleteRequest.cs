using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Lessons.Delete;

public class LessonDeleteRequest : IRequestModel
{
    public Guid Id { get; init; }
}
