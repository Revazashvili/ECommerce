using EventBus;

namespace Payment.API.IntegrationEvents.Events;

public class OrderPaymentFailedIntegrationEvent(Guid orderNumber) : IntegrationEvent
{
    public Guid OrderNumber { get; } = orderNumber;
}