using Contracts.Mediatr.Wrappers;
using EventBridge;
using EventBridge.Dispatcher;
using Ordering.Domain.Events;

namespace Ordering.Application.Features.PlaceOrder;

public class OrderPlacedDomainEventHandler(IIntegrationEventDispatcher dispatcher) 
    : INotificationHandler<OrderPlacedDomainEvent>
{
    public Task Handle(OrderPlacedDomainEvent notification, CancellationToken cancellationToken) => 
        dispatcher.DispatchAsync("OrderPlaced", new OrderPlacedIntegrationEvent(notification.UserId), cancellationToken);
}