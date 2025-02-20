using Contracts.Mediatr.Wrappers;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.PlaceOrder;

public record PlaceOrderCommand(PlaceOrderCommandAddressDto Address,List<PlaceOrderCommandBasketItem> BasketItems) 
    : IValidatedCommand<Order>;
    
public class PlaceOrderCommandAddressDto
{
    public string City { get; set; }
    public string Street { get; set; }
    public string? State { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }

    public Address ToAddress() => new(Street, City, State, Country, ZipCode);
}

public class PlaceOrderCommandBasketItem
{
    public required Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
    public required string PictureUrl { get; set; }
}