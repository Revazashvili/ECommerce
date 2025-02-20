using FluentValidation;
using Ordering.Application.Models;

namespace Ordering.Application.PlaceOrder;

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