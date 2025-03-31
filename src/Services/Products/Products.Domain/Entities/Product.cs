namespace Products.Domain.Entities;

public class Product : Entity
{
    public static Product Create(Guid id, 
        string name,
        double price,
        string imageUrl, 
        List<ProductCategory> categories)
    {
        return new Product
        {
            Id = id,
            Name = name,
            Price = price,
            ImageUrl = imageUrl,
            Categories = categories
        };
    }
    
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required double Price { get; init; }
    public required string ImageUrl { get; init; }
    public required List<ProductCategory> Categories { get; init; }
}