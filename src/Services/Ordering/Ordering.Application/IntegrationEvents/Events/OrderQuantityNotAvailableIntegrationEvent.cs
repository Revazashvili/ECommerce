using EventBus;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderQuantityNotAvailableIntegrationEvent : IntegrationEvent
{
    public OrderQuantityNotAvailableIntegrationEvent(Guid orderNumber)
    {
        OrderNumber = orderNumber;
    }

    public Guid OrderNumber { get; }
}