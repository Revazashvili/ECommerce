using EventBridge;
using EventBridge.Subscriber;
using MediatR;
using Ordering.Application.Features.CancelOrder;

namespace Ordering.Application.IntegrationEvents;

public class OrderPaymentFailedIntegrationEvent : IntegrationEvent;

public class OrderPaymentFailedIntegrationEventHandler(ISender sender) : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
{
    public Task HandleAsync(OrderPaymentFailedIntegrationEvent @event, CancellationToken cancellationToken) => 
        sender.Send(new CancelOrderCommand(Guid.Parse(@event.AggregateId)), cancellationToken);
}