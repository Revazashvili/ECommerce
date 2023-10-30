using EventBus;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderPlaceStartedIntegrationEvent : IntegrationEvent
{
    public OrderPlaceStartedIntegrationEvent(int userId) => UserId = userId;

    public int UserId { get; }
}