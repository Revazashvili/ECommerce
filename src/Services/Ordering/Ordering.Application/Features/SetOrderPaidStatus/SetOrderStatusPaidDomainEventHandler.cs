using Contracts.Mediatr.Wrappers;
using EventBridge;
using EventBridge.Dispatcher;
using Ordering.Domain.Events;

namespace Ordering.Application.Features.SetOrderPaidStatus;

public class SetOrderStatusPaidDomainEventHandler(IIntegrationEventDispatcher dispatcher)
    : INotificationHandler<SetOrderStatusPaidDomainEvent>
{
    public Task Handle(SetOrderStatusPaidDomainEvent notification, CancellationToken cancellationToken) =>
        dispatcher.DispatchAsync("OrderStatusSetPaid", new SetOrderStatusPaidIntegrationEvent(notification.Order), cancellationToken);
}