using CourseAI.Application.Models;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace CourseAI.Api.Extensions;

public static class OneOfExtensions
{
    public static ActionResult MatchResponse<TResponse>(this OneOf<TResponse, Error> oneOf, Func<TResponse, ActionResult> successFunc)
    {
        return oneOf.Match(successFunc, BuildErrorDetails);
    }

    public static ActionResult MatchResponse<TResponse>(this OneOf<TResponse, Error> oneOf, Func<ActionResult> successFunc)
    {
        return oneOf.Match<ActionResult>(_ => successFunc(), BuildErrorDetails);
    }

    public static ActionResult MatchEmptyResponse<TResponse>(this OneOf<TResponse, Error> oneOf)
    {
        return oneOf.Match<ActionResult>(_ => new NoContentResult(), BuildErrorDetails);
    }

    public static ActionResult MatchResponse<TResponse>(this OneOf<TResponse, Error> oneOf, int successStatusCode)
    {
        return oneOf.Match<ActionResult>(
            _ => new StatusCodeResult(successStatusCode),
            BuildErrorDetails
        );
    }

    private static ObjectResult BuildErrorDetails(Error error) => new(new ProblemDetails
    {
        // Detail = error.Message,
        // Instance = error.Instance,
        Status = error.Status,
        // Title = title,
        Type = error.Type,
        Extensions = new Dictionary<string, object?>
        {
            { "message", error.Message },
            { "errors", error.Errors }
        }
    });
}