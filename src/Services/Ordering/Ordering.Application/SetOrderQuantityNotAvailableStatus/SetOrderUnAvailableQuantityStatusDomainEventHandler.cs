using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.SetOrderQuantityNotAvailableStatus;

public class SetOrderUnAvailableQuantityStatusDomainEventHandler(
        ILogger<SetOrderUnAvailableQuantityStatusDomainEventHandler> logger,
        IOrderRepository orderRepository)
    : INotificationHandler<SetOrderUnAvailableQuantityStatusDomainEvent>
{
    public async Task Handle(SetOrderUnAvailableQuantityStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetByOrderNumberAsync(notification.OrderNumber, cancellationToken);

            if (order is null)
                throw new OrderingException($"Can't find order with OrderNumber: {notification.OrderNumber}");
            
            logger.LogInformation($"Simulate notifying user: {order.UserId} about canceling order with number: {order.OrderNumber}");
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error while publishing event {Event}",nameof(SetOrderPendingStatusIntegrationEvent));
        }
    }
}