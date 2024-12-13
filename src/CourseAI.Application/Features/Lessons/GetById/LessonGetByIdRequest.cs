using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;

namespace CourseAI.Application.Features.Lessons.GetById;

public class LessonGetByIdRequest : IRequestModel<LessonModel>
{
    public Guid Id { get; init; }
}
