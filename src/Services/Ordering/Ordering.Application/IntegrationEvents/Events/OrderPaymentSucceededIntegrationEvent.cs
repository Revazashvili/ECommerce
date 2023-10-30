using EventBus;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderPaymentSucceededIntegrationEvent : IntegrationEvent
{
    public Guid OrderNumber { get; set; }
}