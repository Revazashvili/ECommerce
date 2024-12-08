namespace Ordering.Application.Models;

public class BasketItem
{
    public required Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
    public required string PictureUrl { get; set; }
}