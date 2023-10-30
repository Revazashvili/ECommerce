using EventBus;
using Ordering.Domain.Entities;

namespace Ordering.Application.IntegrationEvents.Events;

public class SetOrderPendingStatusIntegrationEvent : IntegrationEvent
{
    public SetOrderPendingStatusIntegrationEvent(Guid orderNumber, List<OrderItem> orderItems)
    {
        OrderNumber = orderNumber;
        OrderItems = orderItems;
    }

    public Guid OrderNumber { get; }
    public List<OrderItem> OrderItems { get; }
}