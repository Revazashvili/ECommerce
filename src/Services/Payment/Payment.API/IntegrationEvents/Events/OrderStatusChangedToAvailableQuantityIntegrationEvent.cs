using EventBus;
using Payment.API.Models;

namespace Payment.API.IntegrationEvents.Events;

public class OrderStatusChangedToAvailableQuantityIntegrationEvent : IntegrationEvent
{
    public int UserId { get; set; }
    public Guid OrderNumber { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}