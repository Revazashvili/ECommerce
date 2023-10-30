using EventBus;
using Payment.API.IntegrationEvents.Events;

namespace Payment.API.IntegrationEvents.EventHandlers;

public class OrderStatusChangedToAvailableQuantityIntegrationEventHandler 
    : IIntegrationEventHandler<OrderStatusChangedToAvailableQuantityIntegrationEvent>
{
    private readonly ILogger<OrderStatusChangedToAvailableQuantityIntegrationEventHandler> _logger;
    private readonly IEventBus _eventBus;

    public OrderStatusChangedToAvailableQuantityIntegrationEventHandler(ILogger<OrderStatusChangedToAvailableQuantityIntegrationEventHandler> logger,
        IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }

    public async Task Handle(OrderStatusChangedToAvailableQuantityIntegrationEvent @event)
    {
        try
        {
            // simulating payment process
            await Task.Delay(TimeSpan.FromSeconds(5));

            IntegrationEvent integrationEvent = RandomBoolean()
                ? new OrderPaymentSucceededIntegrationEvent(@event.OrderNumber)
                : new OrderPaymentFailedIntegrationEvent(@event.OrderNumber);

            await _eventBus.PublishAsync(integrationEvent);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}",
                nameof(OrderStatusChangedToAvailableQuantityIntegrationEventHandler));
        }
    }
    
    private static bool RandomBoolean()
    {
        var random = new Random();
        return random.Next() > int.MaxValue / 2;
    }
}