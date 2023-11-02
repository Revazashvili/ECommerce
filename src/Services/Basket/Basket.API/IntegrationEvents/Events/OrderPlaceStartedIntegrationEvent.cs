using EventBus;

namespace Basket.API.IntegrationEvents.Events;

public class OrderPlaceStartedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
}