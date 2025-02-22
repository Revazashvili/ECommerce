using EventBridge;

namespace Products.Application.Features.CheckProductsQuantityAvailabilityCommand;

public class OrderQuantityAvailableIntegrationEvent(Guid orderNumber) : IntegrationEvent(orderNumber.ToString());