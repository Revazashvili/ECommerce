using EventBus;

namespace Payment.API.IntegrationEvents.Events;

public class OrderPaymentSucceededIntegrationEvent : IntegrationEvent
{
    public OrderPaymentSucceededIntegrationEvent(Guid orderNumber)
    {
        OrderNumber = orderNumber;
    }

    public Guid OrderNumber { get; }
}