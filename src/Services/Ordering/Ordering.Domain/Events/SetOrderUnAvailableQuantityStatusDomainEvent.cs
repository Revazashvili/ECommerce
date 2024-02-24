using Contracts.Mediatr.Wrappers;

namespace Ordering.Domain.Events;

public class SetOrderUnAvailableQuantityStatusDomainEvent(Guid orderNumber) : INotification
{
    public Guid OrderNumber { get; } = orderNumber;
}