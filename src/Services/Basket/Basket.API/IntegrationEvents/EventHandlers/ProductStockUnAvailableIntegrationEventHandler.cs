using Basket.API.Grains;
using Basket.API.IntegrationEvents.Events;
using Basket.API.Interfaces;
using EventBus;

namespace Basket.API.IntegrationEvents.EventHandlers;

public class ProductStockUnAvailableIntegrationEventHandler(
        ILogger logger, IBasketRepository basketRepository,
        IGrainFactory grainFactory)
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
                
                // notify user through web sockets that product is removed from basket
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(ProductStockUnAvailableIntegrationEventHandler));
        }
    }
}