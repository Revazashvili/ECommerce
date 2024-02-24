using Contracts.Mediatr.Wrappers;
using EventBus;
using Microsoft.Extensions.Logging;
using Products.Application.IntegrationEvents.Events;
using Products.Domain.Events;

namespace Products.Application.Products;

public class ProductStockUnAvailableDomainEventHandler(ILogger<ProductStockUnAvailableDomainEventHandler> logger,
        IEventBus eventBus)
    : INotificationHandler<ProductStockUnAvailableDomainEvent>
{
    public async Task Handle(ProductStockUnAvailableDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var productStockUnAvailableIntegrationEvent =
                new ProductStockUnAvailableIntegrationEvent(notification.Product.Id);
            await eventBus.PublishAsync(productStockUnAvailableIntegrationEvent);
            
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error while publishing event {Event}",nameof(ProductStockUnAvailableIntegrationEvent));
        }
    }
}