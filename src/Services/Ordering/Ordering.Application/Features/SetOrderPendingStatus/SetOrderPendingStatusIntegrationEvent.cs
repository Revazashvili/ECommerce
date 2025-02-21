using EventBridge;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.SetOrderPendingStatus;

public class SetOrderPendingStatusIntegrationEvent(Guid orderNumber, List<OrderItem> orderItems) 
    : IntegrationEvent(orderNumber.ToString())
{
    public List<OrderItem> OrderItems { get; } = orderItems;
}