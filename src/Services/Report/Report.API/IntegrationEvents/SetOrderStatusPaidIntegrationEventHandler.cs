using EventBridge;
using EventBridge.Subscriber;
using Report.API.Models;
using Report.API.Repositories;

namespace Report.API.IntegrationEvents;

public class SetOrderStatusPaidIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public Address Address { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime OrderingDate { get; set; }
}

public class SetOrderStatusPaidIntegrationEventHandler(ILogger<SetOrderStatusPaidIntegrationEventHandler> logger,
        IOrderRepository orderRepository)
    : IIntegrationEventHandler<SetOrderStatusPaidIntegrationEvent>
{
    public async Task HandleAsync(SetOrderStatusPaidIntegrationEvent @event, CancellationToken cancellationToken)
    {
        try
        {
            var order = new Order
            {
                OrderNumber = Guid.Parse(@event.AggregateId),
                UserId = @event.UserId,
                OrderStatus = @event.OrderStatus,
                OrderingDate = @event.OrderingDate,
                Address = @event.Address,
                OrderItems = @event.OrderItems
            };
            
            await orderRepository.AddAsync(order, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error while publishing event {Event}",nameof(SetOrderStatusPaidIntegrationEvent));
        }
    }
}