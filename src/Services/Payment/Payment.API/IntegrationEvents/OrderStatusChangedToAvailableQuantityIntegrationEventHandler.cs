using EventBridge;
using EventBridge.Dispatcher;
using EventBridge.Subscriber;
using Payment.API.Models;

namespace Payment.API.IntegrationEvents;

public class OrderStatusChangedToAvailableQuantityIntegrationEventHandler(IIntegrationEventDispatcher dispatcher)
    : IIntegrationEventHandler<OrderStatusChangedToAvailableQuantityIntegrationEvent>
{
    private readonly Random _random = new();
    public async Task HandleAsync(OrderStatusChangedToAvailableQuantityIntegrationEvent @event, CancellationToken cancellationToken)
    {
        // simulating payment process
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);

        if (_random.Next() > int.MaxValue / 2)
            await dispatcher.DispatchAsync(
                "OrderPaymentSucceeded",
                new OrderPaymentSucceededIntegrationEvent(Guid.Parse(@event.AggregateId)),
                cancellationToken
            );
        else
            await dispatcher.DispatchAsync(
                "OrderPaymentFailed",
                new OrderPaymentFailedIntegrationEvent(Guid.Parse(@event.AggregateId)),
                cancellationToken
            );
    }
}

public class OrderStatusChangedToAvailableQuantityIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}

public class OrderPaymentFailedIntegrationEvent(Guid orderNumber) : IntegrationEvent(orderNumber.ToString());

public class OrderPaymentSucceededIntegrationEvent(Guid orderNumber) : IntegrationEvent(orderNumber.ToString());
