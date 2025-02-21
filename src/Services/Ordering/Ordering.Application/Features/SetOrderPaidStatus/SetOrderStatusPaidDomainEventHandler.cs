using Contracts.Mediatr.Wrappers;
using EventBridge;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;

namespace Ordering.Application.Features.SetOrderPaidStatus;

public class SetOrderStatusPaidDomainEventHandler(IIntegrationEventDispatcher dispatcher)
    : INotificationHandler<SetOrderStatusPaidDomainEvent>
{
    public Task Handle(SetOrderStatusPaidDomainEvent notification, CancellationToken cancellationToken) =>
        dispatcher.DispatchAsync(new SetOrderStatusPaidIntegrationEvent(notification.Order), cancellationToken);
}