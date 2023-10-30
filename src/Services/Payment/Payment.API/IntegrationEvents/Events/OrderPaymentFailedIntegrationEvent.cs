using EventBus;

namespace Payment.API.IntegrationEvents.Events;

public class OrderPaymentFailedIntegrationEvent : IntegrationEvent
{
    public OrderPaymentFailedIntegrationEvent(Guid orderNumber)
    {
        OrderNumber = orderNumber;
    }

    public Guid OrderNumber { get; }
}