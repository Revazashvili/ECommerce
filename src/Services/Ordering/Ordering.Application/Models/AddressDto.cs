using Ordering.Domain.Entities;

namespace Ordering.Application.Models;

public class AddressDto
{
    public string City { get; set; }
    public string Street { get; set; }
    public string? State { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }

    public Address ToAddress() => new(Street, City, State, Country, ZipCode);
}