using Basket.API.Grains;
using Basket.API.IntegrationEvents.Events;
using Basket.API.Interfaces;
using EventBus;
using MessageBus;

namespace Basket.API.IntegrationEvents.EventHandlers;

public class ProductStockUnAvailableIntegrationEventHandler(
        ILogger<ProductStockUnAvailableIntegrationEventHandler> logger,
        IBasketRepository basketRepository,
        IGrainFactory grainFactory,
        IMessageBus messageBus)
    : IIntegrationEventHandler<ProductStockUnAvailableIntegrationEvent>
{
    public async Task Handle(ProductStockUnAvailableIntegrationEvent @event)
    {
        try
        {
            var keys = await basketRepository.GetAllKeysAsync();
            foreach (var key in keys)
            {
                var basketGrain = grainFactory.GetGrain<IBasketGrain>(key);
                await basketGrain.RemoveItemsByProductId(@event.ProductId);

                await messageBus.PublishAsync($"PRODUCT_REMOVED_FROM_BASKET_{@event.ProductId}");
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(ProductStockUnAvailableIntegrationEventHandler));
        }
    }
}