using EventBridge.Subscriber;
using MediatR;
using Ordering.Application.Features.CancelOrder;

namespace Ordering.Application.Features.ReserveFailed;

public class ProductsReserveFailedIntegrationEventHandler : IIntegrationEventHandler<ProductsReserveFailedIntegrationEvent>
{
    private readonly ISender _sender;

    public ProductsReserveFailedIntegrationEventHandler(ISender sender)
    {
        _sender = sender;
    }

    public Task HandleAsync(ProductsReserveFailedIntegrationEvent @event, CancellationToken cancellationToken) =>
        _sender.Send(new CancelOrderCommand(Guid.Parse(@event.AggregateId)), cancellationToken);
}