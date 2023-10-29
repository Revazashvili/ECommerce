using EventBus;
using Products.Domain.Entities;

namespace Products.Application.IntegrationEvents.Events;

public class ProductStockUnAvailableIntegrationEvent : IntegrationEvent
{
    public ProductStockUnAvailableIntegrationEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}