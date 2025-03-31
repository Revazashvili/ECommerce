using EventBridge;

namespace Payment.API.ProductsReserved;

public class OrderPaymentSucceededIntegrationEvent(Guid orderNumber) : IntegrationEvent(orderNumber.ToString());