namespace CourseAI.Api.Extensions;

public class TokenLoggingMiddleware(RequestDelegate next, ILogger<TokenLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers["Authorization"].ToString();
            logger.LogInformation($"Authorization Header: {token}");
        }
        else
        {
            logger.LogWarning($"Authorization Header not found. {context.Request.Path}");
        }

        await next(context);
    }
}
