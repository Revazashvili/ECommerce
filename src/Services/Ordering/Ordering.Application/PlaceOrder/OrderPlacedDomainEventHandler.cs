using Contracts.Mediatr.Wrappers;
using EventBridge;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;

namespace Ordering.Application.PlaceOrder;

public class OrderPlacedDomainEventHandler(IIntegrationEventDispatcher dispatcher) 
    : INotificationHandler<OrderPlacedDomainEvent>
{
    public Task Handle(OrderPlacedDomainEvent notification, CancellationToken cancellationToken) => 
        dispatcher.DispatchAsync(new OrderPlaceStartedIntegrationEvent(notification.UserId), cancellationToken);
}