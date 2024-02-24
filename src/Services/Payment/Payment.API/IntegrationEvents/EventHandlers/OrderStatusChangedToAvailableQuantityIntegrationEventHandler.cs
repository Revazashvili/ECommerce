using EventBus;
using Payment.API.IntegrationEvents.Events;

namespace Payment.API.IntegrationEvents.EventHandlers;

public class OrderStatusChangedToAvailableQuantityIntegrationEventHandler(
        ILogger<OrderStatusChangedToAvailableQuantityIntegrationEventHandler> logger,
        IEventBus eventBus)
    : IIntegrationEventHandler<OrderStatusChangedToAvailableQuantityIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToAvailableQuantityIntegrationEvent @event)
    {
        try
        {
            // simulating payment process
            await Task.Delay(TimeSpan.FromSeconds(5));

            if (RandomBoolean())
                await eventBus.PublishAsync(new OrderPaymentSucceededIntegrationEvent(@event.OrderNumber));
            else
                await eventBus.PublishAsync(new OrderPaymentFailedIntegrationEvent(@event.OrderNumber));
            
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(OrderStatusChangedToAvailableQuantityIntegrationEventHandler));
        }
    }
    
    private static bool RandomBoolean()
    {
        var random = new Random();
        return random.Next() > int.MaxValue / 2;
    }
}