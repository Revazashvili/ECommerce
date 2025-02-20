using Contracts.Mediatr.Wrappers;
using EventBus;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.Features.SetOrderQuantityAvailableStatus;

public class SetOrderAvailableQuantityStatusDomainEventHandler(
        ILogger<SetOrderAvailableQuantityStatusDomainEventHandler> logger,
        IOrderRepository orderRepository, IEventBus eventBus)
    : INotificationHandler<SetOrderAvailableQuantityStatusDomainEvent>
{
    public async Task Handle(SetOrderAvailableQuantityStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetByOrderNumberAsync(notification.OrderNumber, cancellationToken);

            if (order is null)
                throw new OrderingException($"Can't find order with OrderNumber: {notification.OrderNumber}");

            var orderStatusChangedToAvailableQuantityIntegrationEvent =
                new OrderStatusChangedToAvailableQuantityIntegrationEvent(order.UserId, order.OrderNumber,
                    order.OrderItems);
            await eventBus.PublishAsync(orderStatusChangedToAvailableQuantityIntegrationEvent);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error while publishing event {Event}",
                nameof(OrderStatusChangedToAvailableQuantityIntegrationEvent));
        }
    }
}