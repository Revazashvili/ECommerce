using Ordering.Domain.Entities;

namespace Ordering.Domain.Exceptions;

public class OrderStatusException : OrderingException
{
    public OrderStatusException(OrderStatus from, OrderStatus to)
        : base($"Can't change the order status from {from} to {to}.")
    {
        
    }
}