namespace Basket.API.Models;

public class Basket
{
    public int UserId { get; set; }
    public List<BasketItem> Items { get; set; } = new();
}