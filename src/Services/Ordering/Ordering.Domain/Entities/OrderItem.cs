using Ordering.Domain.Exceptions;
using Services.Common.Domain;

namespace Ordering.Domain.Entities;

public class OrderItem : Entity
{
    private OrderItem() { }
    private OrderItem(Guid productId, string productName, decimal price, int quantity, string pictureUrl)
    {
        if (quantity <= 0)
            throw new InvalidQuantityException();
        
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
        PictureUrl = pictureUrl;
    }

    public static OrderItem Create(Guid productId,
        string productName,decimal price,
        int quantity, string pictureUrl)
    {
        return new OrderItem(productId, productName, price, quantity, pictureUrl);
    }
    
    public Guid Id { get; set; }
    public Guid ProductId { get; }
    public string ProductName { get; }
    public decimal Price { get; }
    public int Quantity { get; private set; }
    public string PictureUrl { get; }

    public void IncreaseQuantity(int quantityToAdd)
    {
        Quantity += quantityToAdd;
    }
}