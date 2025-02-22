using Contracts.Mediatr.Wrappers;
using EventBridge.Dispatcher;
using Products.Domain.Events;

namespace Products.Application.Features.UpdateProductStock;

public class ProductStockUnavailableDomainEventHandler(IIntegrationEventDispatcher dispatcher)
    : INotificationHandler<ProductStockUnavailableDomainEvent>
{
    public Task Handle(ProductStockUnavailableDomainEvent notification, CancellationToken cancellationToken)
    {
        var @event = new ProductStockUnavailableIntegrationEvent(notification.Product.Id);
        return dispatcher.DispatchAsync("ProductStockUnavailable", @event, cancellationToken);
    }
}