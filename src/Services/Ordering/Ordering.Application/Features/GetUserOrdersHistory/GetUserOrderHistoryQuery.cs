using Contracts.Mediatr.Wrappers;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.GetUserOrdersHistory;

public record GetUserOrderHistoryQuery : IValidatedQuery<IEnumerable<Order>>;