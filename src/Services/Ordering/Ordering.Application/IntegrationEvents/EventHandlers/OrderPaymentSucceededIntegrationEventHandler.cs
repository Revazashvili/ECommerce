using EventBus;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.Orders;

namespace Ordering.Application.IntegrationEvents.EventHandlers;

public class OrderPaymentSucceededIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>
{
    private readonly ILogger<OrderPaymentSucceededIntegrationEventHandler> _logger;
    private readonly ISender _sender;

    public OrderPaymentSucceededIntegrationEventHandler(
        ILogger<OrderPaymentSucceededIntegrationEventHandler> logger,ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }
    
    public async Task Handle(OrderPaymentSucceededIntegrationEvent @event)
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