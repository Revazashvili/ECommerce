using EventBus;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.SetOrderQuantityAvailableStatus;

namespace Ordering.Application.IntegrationEvents.EventHandlers;

public class OrderQuantityAvailableIntegrationEventHandler(
        ILogger<OrderQuantityAvailableIntegrationEventHandler> logger, ISender sender)
    : IIntegrationEventHandler<OrderQuantityAvailableIntegrationEvent>
{
    public async Task Handle(OrderQuantityAvailableIntegrationEvent @event)
    {
        try
        {
            logger.LogInformation("Handling Event: {@Event}", @event);
            await sender.Send(new SetOrderQuantityAvailableStatusCommand(@event.OrderNumber));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(OrderQuantityAvailableIntegrationEventHandler));
        }
    }
}