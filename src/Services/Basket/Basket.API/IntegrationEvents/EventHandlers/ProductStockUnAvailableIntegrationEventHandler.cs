using Basket.API.IntegrationEvents.Events;
using Basket.API.Interfaces;
using EventBus;

namespace Basket.API.IntegrationEvents.EventHandlers;

public class ProductStockUnAvailableIntegrationEventHandler : IIntegrationEventHandler<ProductStockUnAvailableIntegrationEvent>
{
    private readonly ILogger<ProductStockUnAvailableIntegrationEventHandler> _logger;
    private readonly IBasketRepository _basketRepository;

    public ProductStockUnAvailableIntegrationEventHandler(ILogger<ProductStockUnAvailableIntegrationEventHandler> logger, IBasketRepository basketRepository)
    {
        _logger = logger;
        _basketRepository = basketRepository;
    }
    
    public async Task Handle(ProductStockUnAvailableIntegrationEvent @event)
    {
        try
        {
            var keys = await _basketRepository.GetAllKeysAsync();
            foreach (var key in keys)
            {
                var basket = await _basketRepository.GetBasketAsync(key, CancellationToken.None);

                if (basket is null)
                    continue;

                basket.Items.RemoveAll(item => item.ProductId == @event.ProductId);
                await _basketRepository.CreateOrUpdateBasketAsync(basket);
                // notify user through web sockets that product is removed from basket
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}",
                nameof(ProductStockUnAvailableIntegrationEventHandler));
        }
    }
}