using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Application.SetOrderQuantityNotAvailableStatus;

public record SetOrderQuantityNotAvailableStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;