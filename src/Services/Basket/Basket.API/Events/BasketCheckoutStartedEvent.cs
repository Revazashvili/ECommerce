using Basket.API.Models;
using EventBus;

namespace Basket.API.Events;

public class BasketCheckoutStartedEvent : IntegrationEvent
{
    public BasketCheckoutStartedEvent(int userId, string orderNumber, Address address, PaymentInfo paymentInfo)
    {
        UserId = userId;
        OrderNumber = orderNumber;
        Address = address;
        PaymentInfo = paymentInfo;
    }

    public int UserId { get; set; }
    public string OrderNumber { get; set; }
    public Address Address { get; set; }
    public PaymentInfo PaymentInfo { get; set; }
}