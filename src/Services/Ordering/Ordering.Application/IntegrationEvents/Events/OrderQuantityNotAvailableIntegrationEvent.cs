using EventBus;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderQuantityNotAvailableIntegrationEvent : IntegrationEvent
{
    public Guid OrderNumber { get; set; }
}