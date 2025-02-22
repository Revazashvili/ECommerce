using EventBus;
using MediatR;
using Products.Application.Features.CheckProductsQuantityAvailabilityCommand;

namespace Products.Application.IntegrationEvents;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; }
    public decimal Price { get; }
    public int Quantity { get; private set; }
    public string PictureUrl { get; }
}

public class SetOrderPendingStatusIntegrationEvent : IntegrationEvent
{
    public Guid OrderNumber { get; set; }
    public List<OrderItem> OrderItems { get;set; }
}

public class SetOrderPendingStatusIntegrationEventHandler(ISender sender)
    : IIntegrationEventHandler<SetOrderPendingStatusIntegrationEvent>
{
    public Task Handle(SetOrderPendingStatusIntegrationEvent @event)
    {
        var products = @event.OrderItems.ToDictionary(x => x.ProductId, x => x.Quantity);
        return sender.Send(new CheckProductsQuantityAvailabilityCommand(@event.OrderNumber, products), CancellationToken.None);
    }
}