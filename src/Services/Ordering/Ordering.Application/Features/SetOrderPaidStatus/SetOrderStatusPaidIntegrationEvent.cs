using EventBridge;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.SetOrderPaidStatus;

public class SetOrderStatusPaidIntegrationEvent : IntegrationEvent
{
    public SetOrderStatusPaidIntegrationEvent(Order order) : base(order.OrderNumber.ToString())
    {
        OrderStatus = order.OrderStatus;
        UserId = order.UserId;
        OrderItems = order.OrderItems;
        Address = order.Address;
        OrderingDate = order.OrderingDate;
    }
    
    public Guid UserId { get; init; }
    public List<OrderItem> OrderItems { get; init; }
    public Address Address { get; init; }
    public OrderStatus OrderStatus { get; set; }
    public DateTimeOffset OrderingDate { get; init; }
}