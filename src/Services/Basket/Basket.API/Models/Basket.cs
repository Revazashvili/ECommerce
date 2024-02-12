namespace Basket.API.Models;

[GenerateSerializer, Alias(nameof(Basket))]
public class Basket
{
    [Id(0)]
    public List<BasketItem> Items { get; set; } = new();
}