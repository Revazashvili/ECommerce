using EventBridge;
using EventBridge.Dispatcher;
using EventBridge.Subscriber;
using Products.Application.Repositories;
using Products.Application.Services;

namespace Products.Application.IntegrationEvents;

public class OrderPlacedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public List<OrderPlacedIntegrationEventOrderItem> OrderItems { get; set; }
}

public class OrderPlacedIntegrationEventOrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class ProductsReservedEvent : IntegrationEvent
{
    public ProductsReservedEvent(Guid orderNumber) : base(orderNumber.ToString()) { }
}

public class ProductsReserveFailedEvent : IntegrationEvent
{
    public ProductsReserveFailedEvent(Guid orderNumber) : base(orderNumber.ToString()) { }
}

public class OrderPlacedIntegrationEventHandler : IIntegrationEventHandler<OrderPlacedIntegrationEvent>
{
    private readonly IInventoryService _inventoryService;
    private readonly IIntegrationEventDispatcher _dispatcher;
    private readonly IProductRepository _productRepository;

    public OrderPlacedIntegrationEventHandler(IInventoryService inventoryService, 
        IIntegrationEventDispatcher dispatcher,
        IProductRepository productRepository)
    {
        _inventoryService = inventoryService;
        _dispatcher = dispatcher;
        _productRepository = productRepository;
    }
    
    public async Task HandleAsync(OrderPlacedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var products = @event.OrderItems
            .Select(orderItem => new ProductToReserve(orderItem.ProductId, orderItem.Quantity))
            .ToList();
        var request = new ReserveRequest(Guid.Parse(@event.AggregateId), products);
        try
        {
            await _inventoryService.ReserveAsync(request, cancellationToken);
            var productsReservedEvent = new ProductsReservedEvent(@event.UserId);
            await _dispatcher.DispatchAsync("ProductsReserved", productsReservedEvent, cancellationToken);
        }
        catch (Exception ex)
        {
            var productsReserveFailedEvent = new ProductsReserveFailedEvent(@event.UserId);
            await _dispatcher.DispatchAsync("ProductsReserveFailed", productsReserveFailedEvent, cancellationToken);
        }
        finally
        {
            await _productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}