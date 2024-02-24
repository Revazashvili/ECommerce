using Contracts.Mediatr.Wrappers;
using Products.Domain.Entities;

namespace Products.Domain.Events;

public class ProductStockUnAvailableDomainEvent(Product product) : INotification
{
    public Product Product { get; } = product;
}