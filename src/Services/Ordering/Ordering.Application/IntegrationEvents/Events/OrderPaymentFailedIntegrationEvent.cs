using EventBus;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderPaymentFailedIntegrationEvent : IntegrationEvent
{
    public Guid OrderNumber { get; set; }
}