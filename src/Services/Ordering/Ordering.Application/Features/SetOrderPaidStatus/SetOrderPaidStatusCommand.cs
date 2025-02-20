using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Application.Features.SetOrderPaidStatus;

public record SetOrderPaidStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;