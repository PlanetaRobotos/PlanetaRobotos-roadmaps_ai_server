namespace CourseAI.Api.Extensions;

public class TokenLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenLoggingMiddleware> _logger;

    public TokenLoggingMiddleware(RequestDelegate next, ILogger<TokenLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers["Authorization"].ToString();
            _logger.LogInformation($"Authorization Header: {token}");
        }
        else
        {
            _logger.LogWarning("Authorization Header not found.");
        }

        await _next(context);
    }
}
