using EventBridge;

namespace Products.Application.Features.OrderPlaced;

public class ProductsReserveFailedIntegrationEvent : IntegrationEvent
{
    public ProductsReserveFailedIntegrationEvent(Guid orderNumber) : base(orderNumber.ToString()) { }
}