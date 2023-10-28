namespace Basket.API.Models;

public class BasketCheckout
{
    public int UserId { get; set; }
    public Address Address { get; set; }
    public PaymentInfo PaymentInfo { get; set; }
}