using Contracts.Mediatr.Wrappers;
using EventBus;
using Microsoft.Extensions.Logging;
using Products.Application.IntegrationEvents.Events;
using Products.Domain.Events;

namespace Products.Application.Products;

public class ProductStockUnAvailableDomainEventHandler : INotificationHandler<ProductStockUnAvailableDomainEvent>
{
    private readonly ILogger<ProductStockUnAvailableDomainEventHandler> _logger;
    private readonly IEventBus _eventBus;

    public ProductStockUnAvailableDomainEventHandler(ILogger<ProductStockUnAvailableDomainEventHandler> logger,IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }
    
    public async Task Handle(ProductStockUnAvailableDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var productStockUnAvailableIntegrationEvent =
                new ProductStockUnAvailableIntegrationEvent(notification.Product.Id);
            await _eventBus.PublishAsync(productStockUnAvailableIntegrationEvent);
            
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error while publishing event {Event}",nameof(ProductStockUnAvailableIntegrationEvent));
        }
    }
}