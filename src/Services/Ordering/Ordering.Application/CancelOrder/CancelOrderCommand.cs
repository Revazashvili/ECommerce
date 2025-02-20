using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Application.CancelOrder;

public record CancelOrderCommand(Guid OrderNumber) : IValidatedCommand<None>;