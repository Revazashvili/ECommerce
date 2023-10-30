using EventBus;

namespace Products.Application.IntegrationEvents.Events;

public class OrderQuantityAvailableIntegrationEvent : IntegrationEvent
{
    public OrderQuantityAvailableIntegrationEvent(Guid orderNumber)
    {
        OrderNumber = orderNumber;
    }

    public Guid OrderNumber { get; }
}