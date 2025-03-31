using EventBridge.Subscriber;
using MediatR;
using Ordering.Application.Features.CancelOrder;

namespace Ordering.Application.Features.PaymentFailed;

public class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
{
    private readonly ISender _sender;

    public OrderPaymentFailedIntegrationEventHandler(ISender sender)
    {
        _sender = sender;
    }
    
    public async Task HandleAsync(OrderPaymentFailedIntegrationEvent @event, CancellationToken cancellationToken) => 
        await _sender.Send(new CancelOrderCommand(Guid.Parse(@event.AggregateId)), cancellationToken);
}