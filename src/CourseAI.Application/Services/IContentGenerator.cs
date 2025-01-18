namespace CourseAI.Application.Services;

public class ImageGenerationResponse
{
    public bool Success { get; set; }
    public string? FileName { get; set; }
    public string? ImageUrl { get; set; }
    public string? Error { get; set; }
}

public interface IContentGenerator
{
    Task<string> GenerateContentAsync(string userPrompt, string systemMessage);

    Task<ImageGenerationResponse> GenerateImageAsync(
        string title,
        bool isRandomPromptWrapper,
        string? style = null,
        string? fileName = null,
        string? savePath = null);
}