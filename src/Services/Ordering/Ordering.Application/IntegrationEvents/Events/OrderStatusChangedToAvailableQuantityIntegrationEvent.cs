using EventBus;
using Ordering.Domain.Entities;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderStatusChangedToAvailableQuantityIntegrationEvent(Guid userId, Guid orderNumber,
    List<OrderItem> orderItems) : IntegrationEvent
{
    public Guid UserId { get; } = userId;
    public Guid OrderNumber { get; } = orderNumber;
    public List<OrderItem> OrderItems { get; } = orderItems;
}