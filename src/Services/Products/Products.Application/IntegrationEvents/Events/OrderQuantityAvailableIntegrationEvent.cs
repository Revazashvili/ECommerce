using EventBus;

namespace Products.Application.IntegrationEvents.Events;

public class OrderQuantityAvailableIntegrationEvent(Guid orderNumber) : IntegrationEvent
{
    public Guid OrderNumber { get; } = orderNumber;
}