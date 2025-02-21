namespace Ordering.Application.IntegrationEvents.Events;

public class OrderPaymentSucceededIntegrationEvent
{
    public Guid OrderNumber { get; set; }
}