using EventBus;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.SetOrderQuantityNotAvailableStatus;

namespace Ordering.Application.IntegrationEvents.EventHandlers;

public class OrderQuantityNotAvailableIntegrationEventHandler(
        ILogger<OrderQuantityNotAvailableIntegrationEventHandler> logger, ISender sender)
    : IIntegrationEventHandler<OrderQuantityNotAvailableIntegrationEvent>
{
    public async Task Handle(OrderQuantityNotAvailableIntegrationEvent @event)
    {
        try
        {
            await sender.Send(new SetOrderQuantityNotAvailableStatusCommand(@event.OrderNumber));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(OrderQuantityNotAvailableIntegrationEventHandler));
        }
    }
}