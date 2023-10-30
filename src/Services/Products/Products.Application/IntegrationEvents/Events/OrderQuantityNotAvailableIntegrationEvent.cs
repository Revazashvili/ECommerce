using EventBus;

namespace Products.Application.IntegrationEvents.Events;

public class OrderQuantityNotAvailableIntegrationEvent : IntegrationEvent
{
    public OrderQuantityNotAvailableIntegrationEvent(Guid orderNumber)
    {
        OrderNumber = orderNumber;
    }

    public Guid OrderNumber { get; }
}