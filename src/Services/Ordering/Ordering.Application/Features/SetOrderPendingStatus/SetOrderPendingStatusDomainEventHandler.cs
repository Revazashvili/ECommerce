using Contracts.Mediatr.Wrappers;
using EventBridge;
using EventBridge.Dispatcher;
using Ordering.Application.Repositories;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;

namespace Ordering.Application.Features.SetOrderPendingStatus;

public class SetOrderPendingStatusDomainEventHandler(
        IOrderRepository orderRepository, IIntegrationEventDispatcher dispatcher)
    : INotificationHandler<SetOrderPendingStatusDomainEvent>
{
    public async Task Handle(SetOrderPendingStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByOrderNumberAsync(notification.OrderNumber, cancellationToken);

        if (order is null)
            throw new OrderingException($"Can't find order with OrderNumber: {notification.OrderNumber}");
            
        var @event = new SetOrderPendingStatusIntegrationEvent(order.OrderNumber,order.OrderItems);
        await dispatcher.DispatchAsync("OrderSetOrderPendingStatus", @event, cancellationToken);
    }
}