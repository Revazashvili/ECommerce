using Contracts.Mediatr.Wrappers;
using Ordering.Domain.Entities;
using Services.Common;

namespace Ordering.Application.GetOrders;

public record GetOrdersQuery(DateTime From,DateTime To,Pagination Pagination) : IValidatedQuery<IEnumerable<Order>>;