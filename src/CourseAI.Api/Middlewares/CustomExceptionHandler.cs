using System.Net;
using CourseAI.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using NeerCore.Exceptions;

namespace CourseAI.Api.Middlewares;

public sealed class CustomExceptionHandler(
    ILogger<CustomExceptionHandler> logger
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        // catch (ValidationException ex)
        // {
        //     await context.Response.WriteJsonAsync(HttpStatusCode.BadRequest, ex.CreateFluentValidationError());
        //     // await ProcessCommonExceptionAsync(context, ex);
        // }
        catch (HttpException ex)
        {
            if (ex.StatusCode >= HttpStatusCode.InternalServerError)
            {
                logger.LogError(ex, "Internal Server Error");
                await context.Response.Write500ErrorAsync(ex, true);
            }
            else
            {
                await context.Response.WriteJsonAsync(ex.StatusCode, new ProblemDetails
                {
                    Detail = ex.Message,
                    Status = (int)ex.StatusCode,
                    Title = ex.ErrorType,
                    Type = ex.GetErrorType(),
                    Extensions = new Dictionary<string, object?>
                    {
                        ["errors"] = ex.Details ?? new Dictionary<string, object>()
                    }
                });
            }
        }
        catch (TaskCanceledException)
        {
            logger.LogInformation("Request {Path} was canceled", context.Request.Path.Value);
        }
        catch (Exception ex)
        {
            await ProcessCommonExceptionAsync(context, ex);
        }
    }

    private async Task ProcessCommonExceptionAsync(HttpContext context, Exception e)
    {
        logger.LogError(e, "Unhandled Server Error");
        await context.Response.Write500ErrorAsync(e, true);
    }
}