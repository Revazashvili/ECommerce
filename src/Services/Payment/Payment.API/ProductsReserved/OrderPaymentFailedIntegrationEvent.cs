using EventBridge;

namespace Payment.API.ProductsReserved;

public class OrderPaymentFailedIntegrationEvent(Guid orderNumber) : IntegrationEvent(orderNumber.ToString());