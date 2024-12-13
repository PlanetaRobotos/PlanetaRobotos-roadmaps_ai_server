namespace Fleet.Domain.Entities.Roadmaps;

public class LessonContent
{
    public string? MainContent { get; set; } // The main lesson body (HTML or Markdown)
    public List<string>? Resources { get; set; } // Links or references for further reading
    public List<string>? Examples { get; set; } // Code examples or snippets
    public List<string>? Quizzes { get; set; } // Optional quizzes or questions
}
