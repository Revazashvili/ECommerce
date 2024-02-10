using Contracts.Mediatr.Validation;
using MediatR;

namespace Contracts.Mediatr.Wrappers;

public interface IValidatedQuery<T> : IRequest<Either<T,ValidationResult>>;