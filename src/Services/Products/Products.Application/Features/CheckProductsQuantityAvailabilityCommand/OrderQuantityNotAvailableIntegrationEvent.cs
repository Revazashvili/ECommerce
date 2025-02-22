using EventBridge;

namespace Products.Application.Features.CheckProductsQuantityAvailabilityCommand;

public class OrderQuantityNotAvailableIntegrationEvent(Guid orderNumber) : IntegrationEvent(orderNumber.ToString());