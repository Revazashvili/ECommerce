using Products.Domain.Events;
using Services.Common.Domain;

namespace Products.Domain.Entities;

public class Product : Entity
{
    private Product() {}
    public Product(Guid id,string name, int quantity, double price, 
        string imageUrl, List<ProductCategory> categories)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Price = price;
        ImageUrl = imageUrl;
        Categories = categories;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string ImageUrl { get; set; }
    public List<ProductCategory> Categories { get; set; }

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