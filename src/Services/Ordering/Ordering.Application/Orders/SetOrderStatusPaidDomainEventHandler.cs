using Contracts.Mediatr.Wrappers;
using EventBus;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders;

public class SetOrderStatusPaidDomainEventHandler : INotificationHandler<SetOrderStatusPaidDomainEvent>
{
    private readonly ILogger<SetOrderStatusPaidDomainEventHandler> _logger;
    private readonly IEventBus _eventBus;

    public SetOrderStatusPaidDomainEventHandler(ILogger<SetOrderStatusPaidDomainEventHandler> logger,IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }

    
    public async Task Handle(SetOrderStatusPaidDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var setOrderStatusPaidIntegrationEvent = new SetOrderStatusPaidIntegrationEvent(notification.Order);

            await _eventBus.PublishAsync(setOrderStatusPaidIntegrationEvent);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error while publishing event {Event}",nameof(SetOrderStatusPaidIntegrationEvent));
        }
    }
}