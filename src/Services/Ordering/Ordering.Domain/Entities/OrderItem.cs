using Ordering.Domain.Exceptions;

namespace Ordering.Domain.Entities;

public class OrderItem : Entity
{
    public static OrderItem Create(Guid productId,
        string productName,decimal price,
        int quantity, string pictureUrl)
    {
        if (quantity <= 0)
            throw new InvalidQuantityException();

        return new OrderItem
        {
            ProductId = productId,
            ProductName = productName,
            Price = price,
            Quantity = quantity,
            PictureUrl = pictureUrl
        };
    }
    
    public Guid Id { get; init; }
    public required Guid ProductId { get; init; }
    public required string ProductName { get; init;  }
    public required decimal Price { get; init; }
    public required int Quantity { get; set; }
    public required string PictureUrl { get; init; }

    public void IncreaseQuantity(int quantityToAdd)
    {
        Quantity += quantityToAdd;
    }
}