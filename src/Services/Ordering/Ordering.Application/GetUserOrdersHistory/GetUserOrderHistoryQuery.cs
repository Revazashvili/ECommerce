using Contracts.Mediatr.Wrappers;
using Ordering.Domain.Entities;

namespace Ordering.Application.GetUserOrdersHistory;

public record GetUserOrderHistoryQuery : IValidatedQuery<IEnumerable<Order>>;