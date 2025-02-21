using EventBridge;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.SetOrderQuantityAvailableStatus;

public class OrderStatusChangedToAvailableQuantityIntegrationEvent(Guid userId, Guid orderNumber,
    List<OrderItem> orderItems) : IntegrationEvent(orderNumber.ToString())
{
    public Guid UserId { get; } = userId;
    public List<OrderItem> OrderItems { get; } = orderItems;
}