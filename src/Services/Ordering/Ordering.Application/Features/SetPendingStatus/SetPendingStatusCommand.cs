using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Application.Features.SetPendingStatus;

public record SetPendingStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;