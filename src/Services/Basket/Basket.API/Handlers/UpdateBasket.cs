using Basket.API.Interfaces;
using Basket.API.Models;
using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;

namespace Basket.API.Handlers;

public record UpdateBasketCommand(Models.Basket Basket) : IValidatedCommand<Models.Basket>;

public class UpdateBasketCommandHandler : IValidatedCommandHandler<UpdateBasketCommand,Models.Basket>
{
    private readonly ILogger<UpdateBasketCommand> _logger;
    private readonly IBasketRepository _basketRepository;
    private static readonly ValidationResult Error = new("Can't update basket");
    
    public UpdateBasketCommandHandler(ILogger<UpdateBasketCommand> logger,IBasketRepository basketRepository)
    {
        _logger = logger;
        _basketRepository = basketRepository;
    }
    
    public async Task<Either<Models.Basket, ValidationResult>> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _basketRepository.CreateOrUpdateBasketAsync(request.Basket);
            var basket = await _basketRepository.GetBasketAsync(request.Basket.UserId.ToString(), cancellationToken);

            return basket is null ? Error : basket;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}", nameof(UpdateBasketCommandHandler));
            return Error;
        }
    }
}

public class UpdateBasketCommandValidator : AbstractValidator<UpdateBasketCommand>
{
    public UpdateBasketCommandValidator()
    {
        RuleFor(command => command.Basket.UserId)
            .NotNull()
            .WithMessage("UserId must not be null.")
            .NotEqual(0)
            .WithMessage("Id must not equal to zero.");

        RuleFor(command => command.Basket.Items)
            .NotNull()
            .WithMessage("Items must not be null.")
            .NotEmpty()
            .WithMessage("Items must not be empty.")
            .ForEach(collection => collection.SetValidator(new BasketItemValidator()));

        RuleForEach(command => command.Basket.Items)
            .SetValidator(model => new BasketItemValidator());

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
            .GreaterThanOrEqualTo(1)
            .WithMessage("ProductId must not equal to or less than zero.");
        
        RuleFor(command => command.ProductName)
            .NotNull()
            .WithMessage("ProductName must not be null.")
            .NotEmpty()
            .WithMessage("ProductName must not be empty.");

    }
}