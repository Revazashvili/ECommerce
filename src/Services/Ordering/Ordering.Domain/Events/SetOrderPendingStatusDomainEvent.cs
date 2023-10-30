using Contracts.Mediatr.Wrappers;

namespace Ordering.Domain.Events;

public class SetOrderPendingStatusDomainEvent : INotification
{
    public SetOrderPendingStatusDomainEvent(Guid orderNumber) => OrderNumber = orderNumber;

    public Guid OrderNumber { get; }
}