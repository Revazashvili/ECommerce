using Contracts.Mediatr.Wrappers;
using Ordering.Domain.Entities;

namespace Ordering.Domain.Events;

public class SetOrderStatusPaidDomainEvent(Order order) : INotification
{
    public Order Order { get; } = order;
}