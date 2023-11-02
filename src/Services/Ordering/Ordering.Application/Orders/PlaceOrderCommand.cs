using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using EventBus;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.Models;
using Ordering.Application.Services;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public record PlaceOrderCommand(AddressDto Address,List<BasketItem> BasketItems) 
    : IValidatedCommand<Order>;
    
public class PlaceOrderCommandHandler : IValidatedCommandHandler<PlaceOrderCommand,Order>
{
    private readonly ILogger<CancelOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;
    private readonly IIdentityService _identityService;

    public PlaceOrderCommandHandler(ILogger<CancelOrderCommandHandler> logger,IOrderRepository orderRepository,
        IEventBus eventBus,IIdentityService identityService)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _eventBus = eventBus;
        _identityService = identityService;
    }

    public async Task<Either<Order, ValidationResult>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _identityService.GetUserId();
            var order = Order.Create(userId, request.Address.ToAddress());
            foreach (var basketItem in request.BasketItems)
            {
                order.AddOrderItem(basketItem.ProductId,
                    basketItem.ProductName, basketItem.Price,
                    basketItem.Quantity, basketItem.PictureUrl);
            }

            await _orderRepository.AddAsync(order);
            await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            await _eventBus.PublishAsync(new OrderPlaceStartedIntegrationEvent(userId));
            
            return order;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {Handler}",nameof(PlaceOrderCommandHandler));
            return new ValidationResult("Can't place order");
        }
    }
}

public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleForEach(command => command.BasketItems)
            .SetValidator(model => new BasketItemValidator());
        
        RuleFor(command => command.Address)
            .NotNull()
            .WithMessage("Address must not be null.")
            .SetValidator(command => new AddressDtoValidator());
    }
}

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(command => command.City)
            .NotNull()
            .WithMessage("City must not be null.")
            .NotEmpty()
            .WithMessage("City must not be empty.");
        
        RuleFor(command => command.Country)
            .NotNull()
            .WithMessage("Country must not be null.")
            .NotEmpty()
            .WithMessage("Country must not be empty.");
        
        RuleFor(command => command.Street)
            .NotNull()
            .WithMessage("Street must not be null.")
            .NotEmpty()
            .WithMessage("Street must not be empty.");
        
        RuleFor(command => command.ZipCode)
            .NotNull()
            .WithMessage("ZipCode must not be null.")
            .NotEmpty()
            .WithMessage("ZipCode must not be empty.");
    }
}

public class BasketItemValidator : AbstractValidator<BasketItem>
{
    public BasketItemValidator()
    {
        RuleFor(command => command.Price)
            .NotNull()
            .WithMessage("Price must not be null.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Price must not equal to or less than zero.");
        
        RuleFor(command => command.Quantity)
            .NotNull()
            .WithMessage("Quantity must not be null.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Quantity must not equal to or less than zero.");
        
        RuleFor(command => command.PictureUrl)
            .NotNull()
            .WithMessage("PictureUrl must not be null.")
            .NotEmpty()
            .WithMessage("PictureUrl must not be empty.");
        
        RuleFor(command => command.ProductId)
            .NotNull()
            .WithMessage("ProductId must not be null.")
            .NotEmpty()
            .WithMessage("ProductId must not be empty.");
        
        RuleFor(command => command.ProductName)
            .NotNull()
            .WithMessage("ProductName must not be null.")
            .NotEmpty()
            .WithMessage("ProductName must not be empty.");

    }
}