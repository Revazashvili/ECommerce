using Contracts.Mediatr.Wrappers;
using EventBridge;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.Features.SetOrderQuantityAvailableStatus;

public class SetOrderAvailableQuantityStatusDomainEventHandler(
        IOrderRepository orderRepository, IIntegrationEventDispatcher dispatcher)
    : INotificationHandler<SetOrderAvailableQuantityStatusDomainEvent>
{
    public async Task Handle(SetOrderAvailableQuantityStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByOrderNumberAsync(notification.OrderNumber, cancellationToken);

        if (order is null)
            throw new OrderingException($"Can't find order with OrderNumber: {notification.OrderNumber}");

        var @event = new OrderStatusChangedToAvailableQuantityIntegrationEvent(order.UserId, order.OrderNumber, order.OrderItems);
        await dispatcher.DispatchAsync("OrderStatusChangedToAvailableQuantity", @event, cancellationToken);
    }
}