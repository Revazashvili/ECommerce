using EventBus;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderQuantityAvailableIntegrationEvent : IntegrationEvent
{
    public Guid OrderNumber { get; set; }
}