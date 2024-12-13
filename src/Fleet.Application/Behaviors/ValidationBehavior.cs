using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Fleet.Application.Models;
using Fleet.Core.Extensions;
using FluentValidation;
using Mediator;
using NeerCore.Exceptions;
using OneOf;

namespace Fleet.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : new()
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly ConcurrentDictionary<Type, bool> IsOneOfTypeCache = new();
    private static readonly ConcurrentDictionary<Type, Action<TResponse, Error>> SetValueActions = new();

    public ValueTask<TResponse> Handle(TRequest message, CancellationToken ct, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var context = new ValidationContext<TRequest>(message);
        var failures = validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        // If there are no validation failures, continue to the next step.
        if (failures.Count == 0)
        {
            return next(message, ct);
        }

        var error = new Error(
            status: 400,
            type: "ValidationFailed",
            message: "One or more validation error occured.",
            errors: failures.ToDictionary(
                k => k.PropertyName.ToCamelCase(),
                v => (object)v.ErrorMessage
            )
        );

        // If the response type is not OneOf<T1, T2, ...>, throw a ValidationFailedException.
        if (!IsOneOfType(typeof(TResponse)))
        {
            throw new ValidationFailedException(error.Message, error.Errors);
        }

        // Otherwise, create a new instance of the response type and set the error value.
        var response = new TResponse();
        var setValueAction = SetValueActions.GetOrAdd(typeof(TResponse), CreateSetValueAction);
        setValueAction(response, error);
        return ValueTask.FromResult(response);
    }

    private static bool IsOneOfType(Type type)
    {
        return IsOneOfTypeCache.GetOrAdd(type, t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(OneOf<>));
    }

    private static Action<TResponse, Error> CreateSetValueAction(Type responseType)
    {
        var valueProperty = responseType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
        if (valueProperty == null)
        {
            throw new InvalidOperationException($"Type {responseType.FullName} does not have a public instance property named 'Value'");
        }

        var responseParam = Expression.Parameter(typeof(TResponse), "response");
        var valueParam = Expression.Parameter(typeof(Error), "value");
        var propertySetter = Expression.Call(responseParam, valueProperty.SetMethod!, valueParam);

        return Expression.Lambda<Action<TResponse, Error>>(propertySetter, responseParam, valueParam).Compile();
    }
}