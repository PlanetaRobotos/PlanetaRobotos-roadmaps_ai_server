namespace CourseAI.Application.Services;

public interface IContentGenerator
{
    Task<string> GenerateContentAsync(string userPrompt, string systemMessage);
}