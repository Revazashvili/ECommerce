using EventBridge;

namespace Products.Application.Features.OrderPlaced;

public class ProductsReservedIntegrationEvent : IntegrationEvent
{
    public ProductsReservedIntegrationEvent(Guid orderNumber) : base(orderNumber.ToString()) { }
}