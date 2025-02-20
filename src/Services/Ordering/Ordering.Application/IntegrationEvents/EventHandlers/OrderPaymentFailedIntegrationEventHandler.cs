using EventBus;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.CancelOrder;
using Ordering.Application.IntegrationEvents.Events;

namespace Ordering.Application.IntegrationEvents.EventHandlers;

public class OrderPaymentFailedIntegrationEventHandler(ILogger<OrderPaymentFailedIntegrationEventHandler> logger,
        ISender sender)
    : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
{
    public async Task Handle(OrderPaymentFailedIntegrationEvent @event)
    {
        try
        {
            logger.LogInformation("Handling Event: {@Event}", @event);
            await sender.Send(new CancelOrderCommand(@event.OrderNumber));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(OrderPaymentFailedIntegrationEventHandler));
        }
    }
}