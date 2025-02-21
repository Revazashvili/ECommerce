namespace Ordering.Application.IntegrationEvents.Events;

public class OrderPaymentFailedIntegrationEvent
{
    public Guid OrderNumber { get; set; }
}