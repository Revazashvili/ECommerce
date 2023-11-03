using EventBus;
using Report.API.Models;

namespace Report.API.IntegrationEvents.Events;

public class SetOrderStatusPaidIntegrationEvent : IntegrationEvent
{
    public Order Order { get; set; }
}