using Contracts.Mediatr.Wrappers;
using EventBus;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public class SetOrderPendingStatusDomainEventHandler : INotificationHandler<SetOrderPendingStatusDomainEvent>
{
    private readonly ILogger<SetOrderPendingStatusDomainEventHandler> _logger;
    private readonly IEventBus _eventBus;
    private readonly IOrderRepository _orderRepository;

    public SetOrderPendingStatusDomainEventHandler(ILogger<SetOrderPendingStatusDomainEventHandler> logger,IEventBus eventBus,
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _eventBus = eventBus;
        _orderRepository = orderRepository;
    }
    
    public async Task Handle(SetOrderPendingStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByOrderNumberAsync(notification.OrderNumber, cancellationToken);

            if (order is null)
                throw new OrderingException($"Can't find order with OrderNumber: {notification.OrderNumber}");
            
            var setOrderPendingStatusIntegrationEvent = new SetOrderPendingStatusIntegrationEvent(
                order.OrderNumber,order.OrderItems);
            await _eventBus.PublishAsync(setOrderPendingStatusIntegrationEvent);
            
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error while publishing event {Event}",nameof(SetOrderPendingStatusIntegrationEvent));
        }
    }
}