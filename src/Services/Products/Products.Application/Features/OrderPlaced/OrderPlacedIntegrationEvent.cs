using EventBridge;

namespace Products.Application.Features.OrderPlaced;

public class OrderPlacedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public List<OrderPlacedIntegrationEventOrderItem> OrderItems { get; set; }
}

public class OrderPlacedIntegrationEventOrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}