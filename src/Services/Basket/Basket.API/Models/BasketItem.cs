namespace Basket.API.Models;

[GenerateSerializer, Alias(nameof(BasketItem))]
public class BasketItem
{
    [Id(0)]
    public Guid ProductId { get; set; }
    [Id(1)]
    public string ProductName { get; set; }
    [Id(2)]
    public decimal Price { get; set; }
    [Id(3)]
    public int Quantity { get; set; }
    [Id(4)]
    public string PictureUrl { get; set; }
}