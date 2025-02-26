using Contracts.Mediatr.Wrappers;
using Ordering.Domain.Entities;
using Services.Common;

namespace Ordering.Application.Features.GetOrders;

public record GetOrdersQuery(DateTime From, DateTime To, int PageNumber, int PageSize) : IValidatedQuery<IEnumerable<Order>>;