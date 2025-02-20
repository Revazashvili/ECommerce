using EventBridge;

namespace Ordering.Application.IntegrationEvents.Events;

public class OrderPlaceStartedIntegrationEvent(Guid userId) : 
    IntegrationEvent<Guid>(userId, "OrderPlaced");