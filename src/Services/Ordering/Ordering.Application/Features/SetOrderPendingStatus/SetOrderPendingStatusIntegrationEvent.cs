using EventBridge;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.SetOrderPendingStatus;

public class SetOrderPendingStatusIntegrationEvent(Guid orderNumber, List<OrderItem> orderItems) 
    : IntegrationEvent<Guid>(orderNumber, "OrderSetOrderPendingStatus")
{
    public List<OrderItem> OrderItems { get; } = orderItems;
}