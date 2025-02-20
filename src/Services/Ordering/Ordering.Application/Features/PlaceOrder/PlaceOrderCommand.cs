using Contracts.Mediatr.Wrappers;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.PlaceOrder;

public record PlaceOrderCommand(AddressDto Address,List<BasketItem> BasketItems) 
    : IValidatedCommand<Order>;