using EventBus;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.Orders;

namespace Ordering.Application.IntegrationEvents.EventHandlers;

public class OrderQuantityAvailableIntegrationEventHandler : IIntegrationEventHandler<OrderQuantityAvailableIntegrationEvent>
{
    private readonly ILogger<OrderQuantityAvailableIntegrationEventHandler> _logger;
    private readonly ISender _sender;

    public OrderQuantityAvailableIntegrationEventHandler(
        ILogger<OrderQuantityAvailableIntegrationEventHandler> logger,ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }
    
    public async Task Handle(OrderQuantityAvailableIntegrationEvent @event)
    {
        try
        {
            await _sender.Send(new SetOrderQuantityAvailableStatusCommand(@event.OrderNumber));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}",
                nameof(OrderQuantityAvailableIntegrationEventHandler));
        }
    }
}