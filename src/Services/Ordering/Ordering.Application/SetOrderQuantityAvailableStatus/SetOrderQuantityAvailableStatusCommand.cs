using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Application.SetOrderQuantityAvailableStatus;

public record SetOrderQuantityAvailableStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;