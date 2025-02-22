using EventBridge;
using EventBridge.Subscriber;
using MediatR;
using Products.Application.Features.CheckProductsQuantityAvailabilityCommand;

namespace Products.Application.IntegrationEvents;

public class OrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; private set; }
}

public class SetOrderPendingStatusIntegrationEvent : IntegrationEvent
{
    public List<OrderItem> OrderItems { get;set; }
}

public class SetOrderPendingStatusIntegrationEventHandler(ISender sender)
    : IIntegrationEventHandler<SetOrderPendingStatusIntegrationEvent>
{
    public Task HandleAsync(SetOrderPendingStatusIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var products = @event.OrderItems.ToDictionary(x => x.ProductId, x => x.Quantity);
        return sender.Send(new CheckProductsQuantityAvailabilityCommand(Guid.Parse(@event.AggregateId), products), cancellationToken);
    }
}