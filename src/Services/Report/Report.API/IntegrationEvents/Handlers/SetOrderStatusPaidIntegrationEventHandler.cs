using EventBus;
using Report.API.IntegrationEvents.Events;
using Report.API.Repositories;

namespace Report.API.IntegrationEvents.Handlers;

public class SetOrderStatusPaidIntegrationEventHandler(ILogger<SetOrderStatusPaidIntegrationEventHandler> logger,
        IOrderRepository orderRepository)
    : IIntegrationEventHandler<SetOrderStatusPaidIntegrationEvent>
{
    public async Task Handle(SetOrderStatusPaidIntegrationEvent @event)
    {
        try
        {
            await orderRepository.AddAsync(@event.Order, CancellationToken.None);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error while publishing event {Event}",nameof(SetOrderStatusPaidIntegrationEvent));
        }
    }
}