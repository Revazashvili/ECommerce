using Contracts.Mediatr.Validation;
using MediatR;

namespace Contracts.Mediatr.Wrappers;

public interface IValidatedQueryHandler<in TRequest, TResponse> :
    IRequestHandler<TRequest, Either<TResponse, ValidationResult>> 
    where TRequest : IValidatedQuery<TResponse> { }