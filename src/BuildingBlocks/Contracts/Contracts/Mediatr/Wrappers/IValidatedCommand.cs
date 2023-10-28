using Contracts.Mediatr.Validation;
using MediatR;

namespace Contracts.Mediatr.Wrappers;

public interface IValidatedCommand<T> : IRequest<Either<T, ValidationResult>> { }