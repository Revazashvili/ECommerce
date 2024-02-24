using Ordering.Domain.Entities;

namespace Ordering.Domain.Exceptions;

public class OrderStatusException(OrderStatus from, OrderStatus to) : OrderingException($"Can't change the order status from {from} to {to}.");