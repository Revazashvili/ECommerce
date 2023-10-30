using Contracts.Mediatr.Wrappers;

namespace Ordering.Domain.Events;

public class SetOrderAvailableQuantityStatusDomainEvent : INotification
{
    public SetOrderAvailableQuantityStatusDomainEvent(Guid orderNumber) => OrderNumber = orderNumber;

    public Guid OrderNumber { get; }
}