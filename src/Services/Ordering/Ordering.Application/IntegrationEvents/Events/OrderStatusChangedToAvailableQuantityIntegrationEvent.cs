using EventBus;
using Ordering.Domain.Entities;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderStatusChangedToAvailableQuantityIntegrationEvent : IntegrationEvent
{
    public OrderStatusChangedToAvailableQuantityIntegrationEvent(Guid userId, Guid orderNumber, List<OrderItem> orderItems)
    {
        UserId = userId;
        OrderNumber = orderNumber;
        OrderItems = orderItems;
    }

    public Guid UserId { get; }
    public Guid OrderNumber { get; }
    public List<OrderItem> OrderItems { get; }
}