using EventBus;

namespace Products.Application.IntegrationEvents.Events;

public class OrderQuantityNotAvailableIntegrationEvent(Guid orderNumber) : IntegrationEvent
{
    public Guid OrderNumber { get; } = orderNumber;
}