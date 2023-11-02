using EventBus;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderPlaceStartedIntegrationEvent : IntegrationEvent
{
    public OrderPlaceStartedIntegrationEvent(Guid userId) => UserId = userId;

    public Guid UserId { get; }
}