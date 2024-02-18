using Basket.API.Grains;
using Basket.API.IntegrationEvents.Events;
using EventBus;

namespace Basket.API.IntegrationEvents.EventHandlers;

public class OrderPlaceStartedIntegrationEventHandler(
        ILogger<OrderPlaceStartedIntegrationEventHandler> logger,
        IGrainFactory grainFactory)
    : IIntegrationEventHandler<OrderPlaceStartedIntegrationEvent>
{
    public async Task Handle(OrderPlaceStartedIntegrationEvent @event)
    {
        try
        {
            var basketGrain = grainFactory.GetGrain<IBasketGrain>(@event.UserId.ToString());
            await basketGrain.DeleteAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(OrderPlaceStartedIntegrationEventHandler));
        }
    }
}