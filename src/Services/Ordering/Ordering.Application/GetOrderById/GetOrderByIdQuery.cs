using Contracts.Mediatr.Wrappers;
using Ordering.Domain.Entities;

namespace Ordering.Application.GetOrderById;

public record GetOrderByOrderNumberQuery(Guid OrderNumber) : IValidatedQuery<Order>;