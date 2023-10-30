using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public class SetOrderUnAvailableQuantityStatusDomainEventHandler : INotificationHandler<SetOrderUnAvailableQuantityStatusDomainEvent>
{
    private readonly ILogger<SetOrderUnAvailableQuantityStatusDomainEventHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public SetOrderUnAvailableQuantityStatusDomainEventHandler(ILogger<SetOrderUnAvailableQuantityStatusDomainEventHandler> logger,
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    
    public async Task Handle(SetOrderUnAvailableQuantityStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByOrderNumberAsync(notification.OrderNumber, cancellationToken);

            if (order is null)
                throw new OrderingException($"Can't find order with OrderNumber: {notification.OrderNumber}");
            
            _logger.LogInformation($"Simulate notifying user: {order.UserId} about canceling order with number: {order.OrderNumber}");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error while publishing event {Event}",nameof(SetOrderPendingStatusIntegrationEvent));
        }
    }
}