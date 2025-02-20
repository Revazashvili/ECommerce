using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Application.Features.SetOrderQuantityNotAvailableStatus;

public record SetOrderQuantityNotAvailableStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;