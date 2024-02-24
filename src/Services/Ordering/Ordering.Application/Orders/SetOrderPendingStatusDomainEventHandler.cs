using Contracts.Mediatr.Wrappers;
using EventBus;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public class SetOrderPendingStatusDomainEventHandler(ILogger<SetOrderPendingStatusDomainEventHandler> logger,
        IEventBus eventBus,
        IOrderRepository orderRepository)
    : INotificationHandler<SetOrderPendingStatusDomainEvent>
{
    public async Task Handle(SetOrderPendingStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetByOrderNumberAsync(notification.OrderNumber, cancellationToken);

            if (order is null)
                throw new OrderingException($"Can't find order with OrderNumber: {notification.OrderNumber}");
            
            var setOrderPendingStatusIntegrationEvent = new SetOrderPendingStatusIntegrationEvent(
                order.OrderNumber,order.OrderItems);
            await eventBus.PublishAsync(setOrderPendingStatusIntegrationEvent);
            
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error while publishing event {Event}",nameof(SetOrderPendingStatusIntegrationEvent));
        }
    }
}