using EventBus;

namespace Products.Application.IntegrationEvents.Events;

public class ProductStockUnAvailableIntegrationEvent : IntegrationEvent
{
    public ProductStockUnAvailableIntegrationEvent(Guid productId)
    {
        ProductId = productId;
    }

    public Guid ProductId { get; }
}