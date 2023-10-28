using Contracts.Mediatr.Validation;
using MediatR;

namespace Contracts.Mediatr.Wrappers;

public interface IValidatedCommandHandler<in TRequest, TResponse> :
    IRequestHandler<TRequest, Either<TResponse, ValidationResult>> 
    where TRequest : IValidatedCommand<TResponse> { }