namespace Fleet.Application.Models;

/// <summary>
///   Record that represents a default HTTP error response.
/// </summary>
public sealed record Error
{
    /// <example>400</example>
    public int Status { get; init; }

    /// <example>ValidationFailed</example>
    public string Type { get; init; } = default!;

    /// <example>Something goes wrong :(</example>
    public string Message { get; init; } = default!;

    /// <summary>
    ///   A set of additional errors.
    /// </summary>
    /// <remarks>
    ///   Recommendation: pass a field name as dictionary key
    ///   and value should be plain <see cref="string"/> if field has only single message
    ///   or an array of <see cref="string"/>s if it has more than one error message.
    /// </remarks>
    public IReadOnlyDictionary<string, object>? Errors { get; init; }


    /// <inheritdoc cref="Error"/>
    public Error(int status, string type, string message, IReadOnlyDictionary<string, object>? errors = null)
    {
        Status = status;
        Type = type;
        Message = message;
        Errors = errors;
    }


    public static Error ValidationFailed(IReadOnlyDictionary<string, object>? errors) => new(
        status: 400,
        type: "ValidationFailed",
        message: "Some fields are invalid",
        errors: errors
    );

    public static Error ValidationFailed(string message, IReadOnlyDictionary<string, object>? errors) => new(
        status: 400,
        type: "ValidationFailed",
        message: message,
        errors: errors
    );

    public static Error Unauthorized() => new(
        status: 401,
        type: "Unauthorized",
        message: "You have to authenticate first"
    );

    public static Error Forbidden() => new(
        status: 403,
        type: "Forbidden",
        message: "You are not allowed to perform this action"
    );

    public static Error NotFound<T>() => new(
        status: 404,
        type: "NotFound",
        message: $"Resource of type {typeof(T).Name} not found"
    );

    public static Error NotFound(string message) => new(
        status: 404,
        type: "NotFound",
        message: message
    );

    public static Error ServerError(string message) => new(
        status: 500,
        type: "ServerError",
        message: message
    );
}