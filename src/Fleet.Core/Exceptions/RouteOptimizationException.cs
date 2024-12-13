using System.Net;
using NeerCore.Exceptions;

namespace Fleet.Core.Exceptions;

public class RouteOptimizationException(string message = "Unknown reason") : HttpException("Route optimization failed:" + message)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
    public override string ErrorType => "RouteOptimizationError";
}