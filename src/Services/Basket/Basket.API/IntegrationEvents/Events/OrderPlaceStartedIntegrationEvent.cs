using EventBus;

namespace Basket.API.IntegrationEvents.Events;

public class OrderPlaceStartedIntegrationEvent : IntegrationEvent
{
    public int UserId { get; set; }
}