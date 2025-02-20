using Contracts.Mediatr.Wrappers;
using EventBus;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;

namespace Ordering.Application.Features.SetOrderPaidStatus;

public class SetOrderStatusPaidDomainEventHandler(ILogger<SetOrderStatusPaidDomainEventHandler> logger,
        IEventBus eventBus)
    : INotificationHandler<SetOrderStatusPaidDomainEvent>
{
    public async Task Handle(SetOrderStatusPaidDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var setOrderStatusPaidIntegrationEvent = new SetOrderStatusPaidIntegrationEvent(notification.Order);

            await eventBus.PublishAsync(setOrderStatusPaidIntegrationEvent);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error while publishing event {Event}",nameof(SetOrderStatusPaidIntegrationEvent));
        }
    }
}