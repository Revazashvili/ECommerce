using EventBus;
using Microsoft.Extensions.Logging;
using Products.Application.IntegrationEvents.Events;
using Products.Domain.Models;

namespace Products.Application.IntegrationEvents.EventHandlers;

public class SetOrderPendingStatusIntegrationEventHandler(ILogger<SetOrderPendingStatusIntegrationEventHandler> logger,
        IProductRepository productRepository, IEventBus eventBus)
    : IIntegrationEventHandler<SetOrderPendingStatusIntegrationEvent>
{
    public async Task Handle(SetOrderPendingStatusIntegrationEvent @event)
    {
        try
        {
            logger.LogInformation("Handling Event: {@Event}", @event);
            var productQuantityMapping = new Dictionary<Guid, bool>();
            foreach (var eventOrderItem in @event.OrderItems)
            {
                var product = await productRepository.GetByIdAsync(eventOrderItem.ProductId, CancellationToken.None);
                var hasEnoughQuantity = product is not null && product.Quantity >= eventOrderItem.Quantity;

                productQuantityMapping.Add(eventOrderItem.ProductId, hasEnoughQuantity);
            }

            if (productQuantityMapping.Any(pair => !pair.Value))
                await eventBus.PublishAsync(new OrderQuantityNotAvailableIntegrationEvent(@event.OrderNumber));
            else
                await eventBus.PublishAsync(new OrderQuantityAvailableIntegrationEvent(@event.OrderNumber));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(SetOrderPendingStatusIntegrationEventHandler));
        }
    }
}