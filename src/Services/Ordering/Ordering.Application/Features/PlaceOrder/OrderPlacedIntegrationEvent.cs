using EventBridge;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.PlaceOrder;

public class OrderPlacedIntegrationEvent : IntegrationEvent
{
    public OrderPlacedIntegrationEvent(Order order) : base(order.OrderNumber.ToString())
    {
        UserId = order.UserId;
        OrderItems = order.OrderItems
            .Select(item => new OrderPlacedIntegrationEventOrderItem(item.ProductId, item.Quantity))
            .ToList();
    }
    
    public Guid UserId { get; private set; }
    public List<OrderPlacedIntegrationEventOrderItem> OrderItems { get; private set; }
}

public class OrderPlacedIntegrationEventOrderItem
{
    public OrderPlacedIntegrationEventOrderItem(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
    
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
}