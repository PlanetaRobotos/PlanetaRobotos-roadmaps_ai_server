using CourseAI.Domain.Entities.Roadmaps;

namespace CourseAI.Application.Models.Roadmaps;

public class LessonModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public bool Completed { get; init; }
    public int Order { get; init; }

    public string? Description { get; set; }
    public LessonContent? Content { get; set; } = null!;
    public IList<QuizModel> Quizzes { get; set; } = new List<QuizModel>();

    public static LessonModel FromEntity(Lesson entity) =>
        new()
        {
            Id = entity.Id,
            Title = entity.Title,
            Completed = entity.Completed,
            Order = entity.Order,
            Description = entity.Description,
            Content = entity.Content,
            Quizzes = entity.Quizzes.Select(QuizModel.FromEntity).ToList()
        };

    public Lesson ToEntity() =>
        new()
        {
            Id = Id,
            Title = Title,
            Completed = Completed,
            Order = Order,
            Description = Description,
            Content = Content,
            Quizzes = Quizzes.Select(q => q.ToEntity()).ToList()
        };
}