namespace Basket.API.Models;

public class PaymentInfo
{
    public string CardNumber { get; set; }
    public string CardHolderName { get; set; }
    public DateTime CardExpiration { get; set; }
    public string CardSecurityNumber { get; set; }
    public CardType CardType { get; set; }
}