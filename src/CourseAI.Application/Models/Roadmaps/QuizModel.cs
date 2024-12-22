using CourseAI.Domain.Entities.Roadmaps;

namespace CourseAI.Application.Models.Roadmaps;

public class QuizModel
{
    public Guid Id { get; init; }
    public string Question { get; init; } = null!;
    public List<string> Answers { get; init; } = null!;
    public int CorrectAnswerIndex { get; init; }

    public static QuizModel FromEntity(Quiz entity) =>
        new()
        {
            Id = entity.Id,
            Question = entity.Question,
            Answers = entity.Answers,
            CorrectAnswerIndex = entity.CorrectAnswerIndex,
        };

    public Quiz ToEntity() =>
        new()
        {
            Id = Id,
            Question = Question,
            Answers = Answers,
            CorrectAnswerIndex = CorrectAnswerIndex,
        };
}
