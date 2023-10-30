using EventBus;
using Ordering.Domain.Entities;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderStatusChangedToAvailableQuantityIntegrationEvent : IntegrationEvent
{
    public OrderStatusChangedToAvailableQuantityIntegrationEvent(int userId, Guid orderNumber, List<OrderItem> orderItems)
    {
        UserId = userId;
        OrderNumber = orderNumber;
        OrderItems = orderItems;
    }

    public int UserId { get; }
    public Guid OrderNumber { get; }
    public List<OrderItem> OrderItems { get; }
}