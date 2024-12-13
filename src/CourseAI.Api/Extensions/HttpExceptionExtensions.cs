using CourseAI.Application.Models;
using FluentValidation;
using NeerCore.Exceptions;

namespace CourseAI.Api.Extensions;

public static class HttpExceptionExtensions
{
    public static Error CreateError(this HttpException e) => new(
        status: (int)e.StatusCode,
        type: e.ErrorType,
        message: e.Message,
        errors: e.Details
    );

    public static Error CreateFluentValidationError(this ValidationException e) => new(
        status: 400,
        type: "ValidationFailed",
        message: "Invalid model received.",
        errors: e.Errors.ToDictionary(ve => ve.PropertyName, ve => ve.ErrorMessage as object)
    );


    public static string GetErrorType(this Exception httpException)
    {
        return httpException switch
        {
            ValidationFailedException => "https://httpstatuses.com/400",
            UnauthorizedException => "https://httpstatuses.com/401",
            ForbidException => "https://httpstatuses.com/403",
            NotFoundException => "https://httpstatuses.com/404",
            ConflictException => "https://httpstatuses.com/409",
            UnprocessableEntityException => "https://httpstatuses.com/422",
            InternalServerException => "https://httpstatuses.com/500",
            _ => "https://httpstatuses.com/500",
        };
    }
}