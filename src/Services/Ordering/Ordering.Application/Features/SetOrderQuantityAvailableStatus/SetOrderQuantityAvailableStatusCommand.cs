using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Application.Features.SetOrderQuantityAvailableStatus;

public record SetOrderQuantityAvailableStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;