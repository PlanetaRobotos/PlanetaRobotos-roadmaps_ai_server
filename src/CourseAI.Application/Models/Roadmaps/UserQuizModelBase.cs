namespace CourseAI.Application.Models.Roadmaps;

public abstract class UserQuizModelBase
{
    public Guid QuizId { get; set; }
    public int AnswerIndex { get; set; }
}

public class UserQuizModel : UserQuizModelBase
{
}
