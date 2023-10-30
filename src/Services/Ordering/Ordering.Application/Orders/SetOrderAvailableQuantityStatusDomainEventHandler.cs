using Contracts.Mediatr.Wrappers;
using EventBus;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public class SetOrderAvailableQuantityStatusDomainEventHandler : INotificationHandler<SetOrderAvailableQuantityStatusDomainEvent>
{
    private readonly ILogger<SetOrderAvailableQuantityStatusDomainEventHandler> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;

    public SetOrderAvailableQuantityStatusDomainEventHandler(ILogger<SetOrderAvailableQuantityStatusDomainEventHandler> logger,
        IOrderRepository orderRepository,IEventBus eventBus)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    public async Task Handle(SetOrderAvailableQuantityStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByOrderNumberAsync(notification.OrderNumber, cancellationToken);

            if (order is null)
                throw new OrderingException($"Can't find order with OrderNumber: {notification.OrderNumber}");

            var orderStatusChangedToAvailableQuantityIntegrationEvent =
                new OrderStatusChangedToAvailableQuantityIntegrationEvent(order.UserId, order.OrderNumber,
                    order.OrderItems);
            await _eventBus.PublishAsync(orderStatusChangedToAvailableQuantityIntegrationEvent);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error while publishing event {Event}",
                nameof(OrderStatusChangedToAvailableQuantityIntegrationEvent));
        }
    }
}