using EventBus;

namespace Products.Application.IntegrationEvents.Events;

public class ProductStockUnAvailableIntegrationEvent(Guid productId) : IntegrationEvent
{
    public Guid ProductId { get; } = productId;
}