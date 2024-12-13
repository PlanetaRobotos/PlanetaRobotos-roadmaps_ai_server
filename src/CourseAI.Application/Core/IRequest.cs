using CourseAI.Application.Models;
using Mediator;
using OneOf;

namespace CourseAI.Application.Core;

public interface IRequestModel : IRequest<OneOf<Unit, Error>>;

public interface IRequestModel<TResponse> : IRequest<OneOf<TResponse, Error>>;
