using EventBridge;
using Ordering.Domain.Entities;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderStatusChangedToAvailableQuantityIntegrationEvent(Guid userId, Guid orderNumber,
    List<OrderItem> orderItems) : IntegrationEvent<Guid>(orderNumber, "OrderStatusChangedToAvailableQuantity")
{
    public Guid UserId { get; } = userId;
    public List<OrderItem> OrderItems { get; } = orderItems;
}