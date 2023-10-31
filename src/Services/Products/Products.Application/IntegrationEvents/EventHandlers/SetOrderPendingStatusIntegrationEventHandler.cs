using EventBus;
using Microsoft.Extensions.Logging;
using Products.Application.IntegrationEvents.Events;
using Products.Domain.Models;

namespace Products.Application.IntegrationEvents.EventHandlers;

public class SetOrderPendingStatusIntegrationEventHandler : IIntegrationEventHandler<SetOrderPendingStatusIntegrationEvent>
{
    private readonly ILogger<SetOrderPendingStatusIntegrationEventHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IEventBus _eventBus;

    public SetOrderPendingStatusIntegrationEventHandler(ILogger<SetOrderPendingStatusIntegrationEventHandler> logger,
        IProductRepository productRepository,IEventBus eventBus)
    {
        _logger = logger;
        _productRepository = productRepository;
        _eventBus = eventBus;
    }
    
    public async Task Handle(SetOrderPendingStatusIntegrationEvent @event)
    {
        try
        {
            _logger.LogInformation("Handling Event: {@Event}", @event);
            var productQuantityMapping = new Dictionary<Guid, bool>();
            foreach (var eventOrderItem in @event.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(eventOrderItem.ProductId, CancellationToken.None);
                var hasEnoughQuantity = product is not null && product.Quantity >= eventOrderItem.Quantity;

                productQuantityMapping.Add(eventOrderItem.ProductId, hasEnoughQuantity);
            }

            if (productQuantityMapping.Any(pair => !pair.Value))
                await _eventBus.PublishAsync(new OrderQuantityNotAvailableIntegrationEvent(@event.OrderNumber));
            else
                await _eventBus.PublishAsync(new OrderQuantityAvailableIntegrationEvent(@event.OrderNumber));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}",
                nameof(SetOrderPendingStatusIntegrationEventHandler));
        }
    }
}