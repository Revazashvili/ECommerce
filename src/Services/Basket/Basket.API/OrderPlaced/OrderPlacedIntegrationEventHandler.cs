using Basket.API.Grains;
using EventBridge.Subscriber;

namespace Basket.API.OrderPlaced;

public class OrderPlacedIntegrationEventHandler(
    ILogger<OrderPlacedIntegrationEventHandler> logger,
    IGrainFactory grainFactory)
    : IIntegrationEventHandler<OrderPlacedIntegrationEvent>
{
    public Task HandleAsync(OrderPlacedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        try
        {
            var basketGrain = grainFactory.GetGrain<IBasketGrain>(@event.AggregateId);
            return basketGrain.DeleteAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(OrderPlacedIntegrationEventHandler));
            return Task.CompletedTask;
        }
    }
}