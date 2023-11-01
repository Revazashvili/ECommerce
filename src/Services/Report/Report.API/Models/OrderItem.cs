namespace Report.API.Models;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set;}
    public string ProductName { get; set;}
    public decimal Price { get; set;}
    public int Quantity { get; set; }
    public string PictureUrl { get; }
}