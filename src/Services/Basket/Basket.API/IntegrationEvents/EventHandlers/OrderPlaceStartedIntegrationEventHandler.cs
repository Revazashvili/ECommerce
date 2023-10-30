using Basket.API.IntegrationEvents.Events;
using Basket.API.Interfaces;
using EventBus;

namespace Basket.API.IntegrationEvents.EventHandlers;

public class OrderPlaceStartedIntegrationEventHandler : IIntegrationEventHandler<OrderPlaceStartedIntegrationEvent>
{
    private readonly ILogger<ProductStockUnAvailableIntegrationEventHandler> _logger;
    private readonly IBasketRepository _basketRepository;

    public OrderPlaceStartedIntegrationEventHandler(ILogger<ProductStockUnAvailableIntegrationEventHandler> logger, 
        IBasketRepository basketRepository)
    {
        _logger = logger;
        _basketRepository = basketRepository;
    }
    
    public async Task Handle(OrderPlaceStartedIntegrationEvent @event)
    {
        try
        {
            await _basketRepository.DeleteBasketAsync(@event.UserId.ToString(), CancellationToken.None);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}",
                nameof(OrderPlaceStartedIntegrationEventHandler));
        }
    }
}