using EventBus;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.Orders;

namespace Ordering.Application.IntegrationEvents.EventHandlers;

public class OrderQuantityNotAvailableIntegrationEventHandler : IIntegrationEventHandler<OrderQuantityNotAvailableIntegrationEvent>
{
    private readonly ILogger<OrderQuantityNotAvailableIntegrationEventHandler> _logger;
    private readonly ISender _sender;

    public OrderQuantityNotAvailableIntegrationEventHandler(
        ILogger<OrderQuantityNotAvailableIntegrationEventHandler> logger,ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }
    
    public async Task Handle(OrderQuantityNotAvailableIntegrationEvent @event)
    {
        try
        {
            await _sender.Send(new SetOrderQuantityNotAvailableStatusCommand(@event.OrderNumber));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}",
                nameof(OrderQuantityNotAvailableIntegrationEventHandler));
        }
    }
}