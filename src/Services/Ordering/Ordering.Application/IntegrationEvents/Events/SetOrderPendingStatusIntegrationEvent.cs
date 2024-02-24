using EventBus;
using Ordering.Domain.Entities;

namespace Ordering.Application.IntegrationEvents.Events;

public class SetOrderPendingStatusIntegrationEvent(Guid orderNumber, List<OrderItem> orderItems) : IntegrationEvent
{
    public Guid OrderNumber { get; } = orderNumber;
    public List<OrderItem> OrderItems { get; } = orderItems;
}