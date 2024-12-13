namespace Fleet.Application.Services;

public interface IContentGenerator
{
    Task<string> GenerateContentAsync(string userPrompt, string systemMessage);
}
