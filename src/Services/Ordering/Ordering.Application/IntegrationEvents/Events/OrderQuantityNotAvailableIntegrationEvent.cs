using EventBridge;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderQuantityNotAvailableIntegrationEvent
{
    public Guid OrderNumber { get; set; }
}