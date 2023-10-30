using Contracts.Mediatr.Wrappers;

namespace Ordering.Domain.Events;

public class SetOrderUnAvailableQuantityStatusDomainEvent : INotification
{
    public SetOrderUnAvailableQuantityStatusDomainEvent(Guid orderNumber) => OrderNumber = orderNumber;

    public Guid OrderNumber { get; }
}