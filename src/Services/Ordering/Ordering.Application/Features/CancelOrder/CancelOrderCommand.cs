using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Application.Features.CancelOrder;

public record CancelOrderCommand(Guid OrderNumber) : IValidatedCommand<None>;