using Contracts.Mediatr.Wrappers;
using Ordering.Domain.Entities;

namespace Ordering.Domain.Events;

public class SetOrderStatusPaidDomainEvent : INotification
{
    public SetOrderStatusPaidDomainEvent(Order order)
    {
        Order = order;
    }

    public Order Order { get; }
}