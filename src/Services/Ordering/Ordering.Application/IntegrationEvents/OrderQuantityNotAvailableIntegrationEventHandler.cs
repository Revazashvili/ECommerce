using EventBridge;
using EventBridge.Subscriber;
using MediatR;
using Ordering.Application.Features.SetOrderQuantityNotAvailableStatus;

namespace Ordering.Application.IntegrationEvents;

public class OrderQuantityNotAvailableIntegrationEvent : IntegrationEvent;

public class OrderQuantityNotAvailableIntegrationEventHandler(ISender sender)
    : IIntegrationEventHandler<OrderQuantityNotAvailableIntegrationEvent>
{
    public Task HandleAsync(OrderQuantityNotAvailableIntegrationEvent @event, CancellationToken cancellationToken) =>
        sender.Send(new SetOrderQuantityNotAvailableStatusCommand(Guid.Parse(@event.AggregateId)), cancellationToken);
}