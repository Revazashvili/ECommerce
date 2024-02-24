using Contracts.Mediatr.Wrappers;

namespace Ordering.Domain.Events;

public class SetOrderPendingStatusDomainEvent(Guid orderNumber) : INotification
{
    public Guid OrderNumber { get; } = orderNumber;
}