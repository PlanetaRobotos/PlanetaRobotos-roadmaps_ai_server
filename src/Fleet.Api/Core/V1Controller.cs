using Fleet.Application.Models;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Core;

[ApiController]
[Asp.Versioning.ApiVersion(Versions.V1)]
[Route(BaseRoute, Name = "[controller]_[action]")]
[Produces("application/json")]
[ProducesResponseType<Error>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<Error>(StatusCodes.Status401Unauthorized)]
public abstract class V1Controller : ControllerBase
{
    protected const string BaseRoute = "v{version:apiVersion}/[controller]";
    protected const string RouteWithActionName = BaseRoute + "/[action]";

    // protected IUserProvider UserProvider => HttpContext.RequestServices.GetRequiredService<IUserProvider>();

    private ILogger? _logger;
    private ISender? _sender;

    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected ILogger Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(GetType());

    protected ActionResult ProblemFromError(Error error)
    {
        return Problem(
            detail: error.Message,
            statusCode: error.Status,
            type: error.Type
        );
    }
}