namespace Products.Application.Models;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid ProductId { get; }
    public string ProductName { get; }
    public decimal Price { get; }
    public int Quantity { get; private set; }
    public string PictureUrl { get; }
}