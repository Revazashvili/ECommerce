using Basket.API.Grains;
using Basket.API.Interfaces;
using EventBridge;
using EventBridge.Subscriber;
using MessageBus;

namespace Basket.API.IntegrationEvents;

public class ProductStockUnAvailableIntegrationEvent : IntegrationEvent;

public class ProductStockUnAvailableIntegrationEventHandler(
        ILogger<ProductStockUnAvailableIntegrationEventHandler> logger,
        IBasketRepository basketRepository,
        IGrainFactory grainFactory,
        IMessageBus messageBus)
    : IIntegrationEventHandler<ProductStockUnAvailableIntegrationEvent>
{
    public async Task HandleAsync(ProductStockUnAvailableIntegrationEvent @event, CancellationToken cancellationToken)
    {
        try
        {
            var keys = await basketRepository.GetAllKeysAsync();
            foreach (var key in keys)
            {
                var basketGrain = grainFactory.GetGrain<IBasketGrain>(key);
                await basketGrain.RemoveItemsByProductId(Guid.Parse(@event.AggregateId));

                await messageBus.PublishAsync($"PRODUCT_REMOVED_FROM_BASKET_{@event.AggregateId}");
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(ProductStockUnAvailableIntegrationEventHandler));
        }
    }
}