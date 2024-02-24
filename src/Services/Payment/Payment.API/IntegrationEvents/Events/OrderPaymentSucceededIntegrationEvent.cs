using EventBus;

namespace Payment.API.IntegrationEvents.Events;

public class OrderPaymentSucceededIntegrationEvent(Guid orderNumber) : IntegrationEvent
{
    public Guid OrderNumber { get; } = orderNumber;
}