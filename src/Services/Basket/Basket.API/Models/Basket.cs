namespace Basket.API.Models;

public class Basket
{
    public List<BasketItem> Items { get; set; } = new();
}