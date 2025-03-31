using Basket.API.Grains;
using Basket.API.Interfaces;
using EventBridge;
using EventBridge.Subscriber;
using MessageBus;

namespace Basket.API.ProductsReserveFailed;

public class ProductsReserveFailedIntegrationEvent : IntegrationEvent;

public class ProductsReserveFailedIntegrationEventHandler : IIntegrationEventHandler<ProductsReserveFailedIntegrationEvent>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IGrainFactory _grainFactory;
    private readonly IMessageBus _messageBus;

    public ProductsReserveFailedIntegrationEventHandler(IBasketRepository basketRepository,
        IGrainFactory grainFactory,
        IMessageBus messageBus)
    {
        _basketRepository = basketRepository;
        _grainFactory = grainFactory;
        _messageBus = messageBus;
    }

    public async Task HandleAsync(ProductsReserveFailedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var keys = await _basketRepository.GetAllKeysAsync();
        foreach (var key in keys)
        {
            var basketGrain = _grainFactory.GetGrain<IBasketGrain>(key);
            await basketGrain.RemoveItemsByProductId(Guid.Parse(@event.AggregateId));

            await _messageBus.PublishAsync($"PRODUCT_REMOVED_FROM_BASKET_{@event.AggregateId}");
        }
    }
}