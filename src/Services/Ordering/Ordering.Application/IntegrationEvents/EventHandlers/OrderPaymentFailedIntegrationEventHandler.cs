using EventBus;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.Orders;

namespace Ordering.Application.IntegrationEvents.EventHandlers;

public class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
{
    private readonly ILogger<OrderPaymentFailedIntegrationEventHandler> _logger;
    private readonly ISender _sender;

    public OrderPaymentFailedIntegrationEventHandler(
        ILogger<OrderPaymentFailedIntegrationEventHandler> logger,ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }
    
    public async Task Handle(OrderPaymentFailedIntegrationEvent @event)
    {
        try
        {
            await _sender.Send(new CancelOrderCommand(@event.OrderNumber));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}",
                nameof(OrderPaymentFailedIntegrationEventHandler));
        }
    }
}