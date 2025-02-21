using EventBridge;
using Ordering.Domain.Entities;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderSetOrderPendingStatusIntegrationEvent(Guid orderNumber, List<OrderItem> orderItems) 
    : IntegrationEvent<Guid>(orderNumber, "OrderSetOrderPendingStatus")
{
    public List<OrderItem> OrderItems { get; } = orderItems;
}