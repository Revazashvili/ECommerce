using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using EventBridge;
using Microsoft.Extensions.Logging;
using Ordering.Application.Services;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;

namespace Ordering.Application.Features.PlaceOrder;

public class PlaceOrderCommandHandler(ILogger<PlaceOrderCommandHandler> logger, IOrderRepository orderRepository, 
    IIdentityService identityService, IIntegrationEventDispatcher dispatcher)
    : IValidatedCommandHandler<PlaceOrderCommand,Order>
{
    public async Task<Either<Order, ValidationResult>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = identityService.GetUserId();
            var order = Order.Create(userId, request.Address.ToAddress());
            foreach (var basketItem in request.BasketItems)
            {
                order.AddOrderItem(basketItem.ProductId,
                    basketItem.ProductName,
                    basketItem.Price,
                    basketItem.Quantity,
                    basketItem.PictureUrl);
            }

            await orderRepository.AddAsync(order);
            
            await orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return order;
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error occured in {Handler}",nameof(PlaceOrderCommandHandler));
            return new ValidationResult("Can't place order");
        }
    }
}