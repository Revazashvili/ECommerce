using Contracts.Mediatr.Wrappers;

namespace Ordering.Domain.Events;

public class OrderCreatedDomainEvent : INotification
{
    public OrderCreatedDomainEvent(int userId) => UserId = userId;
    
    public int UserId { get; }
}