using Contracts.Mediatr.Wrappers;
using EventBus;
using Microsoft.Extensions.Logging;
using Products.Domain.Events;

namespace Products.Application.Features.UpdateProductStock;

public class ProductStockUnavailableDomainEventHandler(ILogger<ProductStockUnavailableDomainEventHandler> logger,
        IEventBus eventBus)
    : INotificationHandler<ProductStockUnavailableDomainEvent>
{
    public async Task Handle(ProductStockUnavailableDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var productStockUnAvailableIntegrationEvent =
                new ProductStockUnavailableIntegrationEvent(notification.Product.Id);
            await eventBus.PublishAsync(productStockUnAvailableIntegrationEvent);
            
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error while publishing event {Event}",nameof(ProductStockUnavailableIntegrationEvent));
        }
    }
}