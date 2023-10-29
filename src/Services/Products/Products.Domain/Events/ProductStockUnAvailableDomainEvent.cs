using Contracts.Mediatr.Wrappers;
using Products.Domain.Entities;

namespace Products.Domain.Events;

public class ProductStockUnAvailableDomainEvent : INotification
{
    public ProductStockUnAvailableDomainEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}