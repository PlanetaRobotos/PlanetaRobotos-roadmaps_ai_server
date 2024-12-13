using CourseAI.Domain.Entities.Roadmaps;
using FluentValidation;

namespace CourseAI.Application.Models.Roadmaps;

public class RoadmapModuleModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public int Order { get; init; }
    public IList<LessonModel> Lessons { get; init; } = new List<LessonModel>();

    public RoadmapModule ToEntity() =>
        new()
        {
            Id = Id != Guid.Empty ? Id : Guid.NewGuid(),
            Title = Title,
            Order = Order,
            Lessons = Lessons.Select(lesson => lesson.ToEntity()).ToList()
        };
}
