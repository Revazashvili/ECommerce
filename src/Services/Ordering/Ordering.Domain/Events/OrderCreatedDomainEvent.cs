using Contracts.Mediatr.Wrappers;

namespace Ordering.Domain.Events;

public class OrderCreatedDomainEvent(int userId) : INotification
{
    public int UserId { get; } = userId;
}