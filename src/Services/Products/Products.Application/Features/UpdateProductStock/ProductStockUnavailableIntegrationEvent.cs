using EventBridge;

namespace Products.Application.Features.UpdateProductStock;

public class ProductStockUnavailableIntegrationEvent(Guid productId) : IntegrationEvent(productId.ToString());