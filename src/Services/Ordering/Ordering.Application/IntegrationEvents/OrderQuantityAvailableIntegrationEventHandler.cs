using EventBridge;
using MediatR;
using Ordering.Application.Features.SetOrderQuantityAvailableStatus;

namespace Ordering.Application.IntegrationEvents;

public class OrderQuantityAvailableIntegrationEvent : IntegrationEvent;

public class OrderQuantityAvailableIntegrationEventHandler(ISender sender)
    : IIntegrationEventHandler<OrderQuantityAvailableIntegrationEvent>
{
    public Task HandleAsync(OrderQuantityAvailableIntegrationEvent @event, CancellationToken cancellationToken) =>
        sender.Send(new SetOrderQuantityAvailableStatusCommand(Guid.Parse(@event.AggregateId)), cancellationToken);
}