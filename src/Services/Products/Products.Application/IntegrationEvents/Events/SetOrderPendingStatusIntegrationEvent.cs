using EventBus;
using Products.Application.Models;

namespace Products.Application.IntegrationEvents.Events;

public class SetOrderPendingStatusIntegrationEvent : IntegrationEvent
{
    public Guid OrderNumber { get; set; }
    public List<OrderItem> OrderItems { get;set; }
}