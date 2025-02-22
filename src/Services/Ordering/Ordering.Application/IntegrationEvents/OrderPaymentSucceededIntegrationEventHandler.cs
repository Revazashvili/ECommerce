using EventBridge;
using EventBridge.Subscriber;
using MediatR;
using Ordering.Application.Features.SetOrderPaidStatus;

namespace Ordering.Application.IntegrationEvents;

public class OrderPaymentSucceededIntegrationEvent : IntegrationEvent;

public class OrderPaymentSucceededIntegrationEventHandler(ISender sender)
    : IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>
{
    public Task HandleAsync(OrderPaymentSucceededIntegrationEvent @event, CancellationToken cancellationToken) =>
        sender.Send(new SetOrderPaidStatusCommand(Guid.Parse(@event.AggregateId)), cancellationToken);
}