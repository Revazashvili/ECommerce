using Contracts.Mediatr.Wrappers;

namespace Ordering.Domain.Events;

public class SetOrderAvailableQuantityStatusDomainEvent(Guid orderNumber) : INotification
{
    public Guid OrderNumber { get; } = orderNumber;
}