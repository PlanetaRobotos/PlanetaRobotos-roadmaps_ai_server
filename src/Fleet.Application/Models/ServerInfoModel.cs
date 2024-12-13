namespace Fleet.Application.Models;

public class ServerInfoModel
{
    /// <example>Fleet</example>
    public required string Name { get; init; }

    /// <example>1.0</example>
    public required string ApiVersion { get; init; }

    /// <example>00000000000000000000000000000000</example>
    public required string BuildId { get; init; }
}