using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Application.SetOrderPaidStatus;

public record SetOrderPaidStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;