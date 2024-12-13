using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NeerCore.Json;

namespace Fleet.Api.Extensions;

public static class HttpContextExtensions
{
    /// <summary>
    ///   Sends a JSON response with provided JSON body and HTTP status code. UTF-8 encoding will be used.
    /// </summary>
    /// <param name="context">Represents the outgoing side of an individual HTTP request.</param>
    /// <param name="statusCode">Contains the values of status codes defined for HTTP.</param>
    /// <param name="response">Response object to be converted and sent as JSON.</param>
    /// <typeparam name="TResponse">Generic response type.</typeparam>
    public static async Task WriteJsonAsync<TResponse>(this HttpResponse context, HttpStatusCode statusCode, TResponse response)
    {
        context.ContentType = "application/json";
        context.StatusCode = (int)statusCode;
        await context.WriteAsync(JsonSerializer.Serialize(response, JsonConventions.CamelCase));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <param name="extended"></param>
    public static async Task Write500ErrorAsync(this HttpResponse context, Exception exception, bool extended = false)
    {
        var message = extended ? exception.Message : "Server Error.";

        await context.WriteJsonAsync(HttpStatusCode.InternalServerError, new ProblemDetails
        {
            Type = exception.GetErrorType(),
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "ServerError",
            Detail = message,
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    public static async Task WriteExtended500ErrorAsync(this HttpResponse context, Exception exception)
    {
        context.ContentType = "text/plain";
        context.StatusCode = StatusCodes.Status500InternalServerError;
        await context.WriteAsync($"===== SERVER ERROR =====\n{exception}\n===== ===== ===== =====");
    }
}