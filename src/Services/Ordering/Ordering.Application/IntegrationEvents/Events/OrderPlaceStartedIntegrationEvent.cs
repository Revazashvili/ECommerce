using EventBus;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderPlaceStartedIntegrationEvent(Guid userId) : IntegrationEvent
{
    public Guid UserId { get; } = userId;
}