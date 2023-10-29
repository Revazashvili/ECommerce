using EventBus;

namespace Basket.API.IntegrationEvents.Events;

public class ProductStockUnAvailableIntegrationEvent : IntegrationEvent
{
    public Guid ProductId { get; set; }
}