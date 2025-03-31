using EventBridge;
using EventBridge.Dispatcher;
using EventBridge.Subscriber;

namespace Payment.API.ProductsReserved;

public class ProductsReservedIntegrationEvent : IntegrationEvent;

public class ProductsReservedIntegrationEventHandler : IIntegrationEventHandler<ProductsReservedIntegrationEvent>
{
    private readonly Random _random;
    private readonly IIntegrationEventDispatcher _dispatcher;
    
    public ProductsReservedIntegrationEventHandler(IIntegrationEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _random = new Random();
    }
    
    public async Task HandleAsync(ProductsReservedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        // simulating payment process
        await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);

        if (_random.Next() > int.MaxValue / 2)
            await _dispatcher.DispatchAsync(
                "OrderPaymentSucceeded",
                new OrderPaymentSucceededIntegrationEvent(Guid.Parse(@event.AggregateId)),
                cancellationToken
            );
        else
            await _dispatcher.DispatchAsync(
                "OrderPaymentFailed",
                new OrderPaymentFailedIntegrationEvent(Guid.Parse(@event.AggregateId)),
                cancellationToken
            );
    }
}