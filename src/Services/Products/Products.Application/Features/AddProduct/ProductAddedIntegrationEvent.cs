using EventBridge;

namespace Products.Application.Features.AddProduct;

public class ProductAddedIntegrationEvent : IntegrationEvent
{
    public ProductAddedIntegrationEvent(Guid productId, string productName) : base(productId.ToString())
    {
        ProductId = productId;
        ProductName = productName;
    }
    
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
}