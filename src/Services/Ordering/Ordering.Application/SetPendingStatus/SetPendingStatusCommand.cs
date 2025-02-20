using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Application.SetPendingStatus;

public record SetPendingStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;