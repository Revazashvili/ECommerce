using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Application.Features.SetOrderPendingStatus;

public record SetOrderPendingStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;