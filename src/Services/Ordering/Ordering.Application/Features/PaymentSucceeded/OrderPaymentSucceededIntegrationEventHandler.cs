using EventBridge.Dispatcher;
using EventBridge.Subscriber;
using Ordering.Application.Repositories;
using Ordering.Domain.Exceptions;

namespace Ordering.Application.Features.PaymentSucceeded;

public class OrderPaymentSucceededIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>
{
    private readonly IOrderRepository _orderRepository;

    public OrderPaymentSucceededIntegrationEventHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task HandleAsync(OrderPaymentSucceededIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByOrderNumberAsync(Guid.Parse(@event.AggregateId), cancellationToken);
        
        if (order is null)
            throw new OrderingException("Order not found");
        
        order.SetPaidStatus();

        await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}