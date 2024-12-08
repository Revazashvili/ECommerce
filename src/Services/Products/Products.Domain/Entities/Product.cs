using Products.Domain.Events;
using Services.Common.Domain;

namespace Products.Domain.Entities;

public class Product : Entity
{
    public static Product Create(Guid id, string name, int quantity, double price,
        string imageUrl, List<ProductCategory> categories)
    {
        return new Product
        {
            Id = id,
            Name = name,
            Quantity = quantity,
            Price = price,
            ImageUrl = imageUrl,
            Categories = categories
        };
    }
    
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required int Quantity { get; set; }
    public required double Price { get; init; }
    public required string ImageUrl { get; init; }
    public required List<ProductCategory> Categories { get; init; }

    public void UpdateQuantity(int quantity)
    {
        const int zero = 0;
        if (quantity < zero)
            throw new Exception(nameof(quantity));
        
        Quantity = quantity;

        if (quantity == zero)
            AddDomainEvent(new ProductStockUnAvailableDomainEvent(this));

    }
}