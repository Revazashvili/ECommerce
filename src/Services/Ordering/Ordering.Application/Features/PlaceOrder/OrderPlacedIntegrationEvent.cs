using EventBridge;

namespace Ordering.Application.Features.PlaceOrder;

public class OrderPlacedIntegrationEvent(Guid userId) : IntegrationEvent(userId.ToString());