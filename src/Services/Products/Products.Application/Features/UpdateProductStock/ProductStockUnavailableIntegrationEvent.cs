using EventBus;

namespace Products.Application.Features.UpdateProductStock;

public class ProductStockUnavailableIntegrationEvent(Guid productId) : IntegrationEvent
{
    public Guid ProductId { get; } = productId;
}