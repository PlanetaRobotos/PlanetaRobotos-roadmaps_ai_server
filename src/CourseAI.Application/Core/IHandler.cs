using CourseAI.Application.Models;
using Mediator;
using OneOf;

namespace CourseAI.Application.Core;

public interface IHandler<in TRequest, TResponse> : IRequestHandler<TRequest, OneOf<TResponse, Error>>
    where TRequest : IRequestModel<TResponse>;

public interface IHandler<in TRequest> : IRequestHandler<TRequest, OneOf<Unit, Error>>
    where TRequest : IRequestModel;