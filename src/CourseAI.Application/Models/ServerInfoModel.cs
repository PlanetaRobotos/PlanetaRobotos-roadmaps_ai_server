namespace CourseAI.Application.Models;

public class ServerInfoModel
{
    /// <example>CourseAI</example>
    public required string Name { get; init; }

    /// <example>1.0</example>
    public required string ApiVersion { get; init; }

    /// <example>00000000000000000000000000000000</example>
    public required string BuildId { get; init; }
}