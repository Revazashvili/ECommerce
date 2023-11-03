using EventBus;
using Report.API.IntegrationEvents.Events;
using Report.API.Repositories;

namespace Report.API.IntegrationEvents.Handlers;

public class SetOrderStatusPaidIntegrationEventHandler : IIntegrationEventHandler<SetOrderStatusPaidIntegrationEvent>
{
    private readonly ILogger<SetOrderStatusPaidIntegrationEventHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public SetOrderStatusPaidIntegrationEventHandler(ILogger<SetOrderStatusPaidIntegrationEventHandler> logger,
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    
    public async Task Handle(SetOrderStatusPaidIntegrationEvent @event)
    {
        try
        {
            await _orderRepository.AddAsync(@event.Order);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error while publishing event {Event}",nameof(SetOrderStatusPaidIntegrationEvent));
        }
    }
}