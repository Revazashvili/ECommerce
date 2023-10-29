using Basket.API.Models;
using EventBus;

namespace Basket.API.IntegrationEvents.Events;

public class BasketCheckoutStartedEvent : IntegrationEvent
{
    public BasketCheckoutStartedEvent(int userId, string orderNumber, 
        Address address, PaymentInfo paymentInfo,
        List<BasketItem> basketItems)
    {
        UserId = userId;
        OrderNumber = orderNumber;
        Address = address;
        PaymentInfo = paymentInfo;
        BasketItems = basketItems;
    }

    public int UserId { get; set; }
    public string OrderNumber { get; set; }
    public Address Address { get; set; }
    public PaymentInfo PaymentInfo { get; set; }
    public List<BasketItem> BasketItems { get; set; }
}