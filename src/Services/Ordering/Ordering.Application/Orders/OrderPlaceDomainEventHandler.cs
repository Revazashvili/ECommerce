using Contracts.Mediatr.Wrappers;
using EventBridge;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders;

public class OrderPlaceDomainEventHandler(IIntegrationEventDispatcher dispatcher) : INotificationHandler<OrderPlaceDomainEvent>
{
    public Task Handle(OrderPlaceDomainEvent notification, CancellationToken cancellationToken)
    {
        return dispatcher.DispatchAsync(new OrderPlaceStartedIntegrationEvent(notification.UserId), cancellationToken);
    }
}