using Basket.API.Events;
using Basket.API.Interfaces;
using Basket.API.Models;
using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using EventBus;
using FluentValidation;

namespace Basket.API.Handlers;

public record BasketCheckoutCommand(BasketCheckout BasketCheckout) : IValidatedCommand<None>;

public class BasketCheckoutCommandHandler : IValidatedCommandHandler<BasketCheckoutCommand, None>
{
    private readonly ILogger<UpdateBasketCommand> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IEventBus _eventBus;

    public BasketCheckoutCommandHandler(ILogger<UpdateBasketCommand> logger,IBasketRepository basketRepository,
        IEventBus eventBus)
    {
        _logger = logger;
        _basketRepository = basketRepository;
        _eventBus = eventBus;
    }
    
    public async Task<Either<None, ValidationResult>> Handle(BasketCheckoutCommand request, CancellationToken cancellationToken)
    {
        var orderNumber = Guid.NewGuid().ToString();
        try
        {
            var basket =await _basketRepository.GetBasketAsync(request.BasketCheckout.UserId.ToString(), cancellationToken);

            if (basket is null)
                return new ValidationResult("Can't find basket");

            var checkoutEvent = new BasketCheckoutStartedEvent(request.BasketCheckout.UserId, orderNumber,
                request.BasketCheckout.Address, request.BasketCheckout.PaymentInfo,request.BasketCheckout.BasketItems);
            
            await _eventBus.PublishAsync(checkoutEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Publishing event: {OrderNumber}", orderNumber);
            return new ValidationResult("Can't publish event");
        }

        return None.Instance;
    }
}

public class BasketCheckoutCommandValidator : AbstractValidator<BasketCheckoutCommand>
{
    public BasketCheckoutCommandValidator()
    {
        RuleFor(command => command.BasketCheckout.UserId)
            .NotNull()
            .WithMessage("UserId must not be null.")
            .NotEqual(0)
            .WithMessage("Id must not equal to zero.");

        RuleFor(command => command.BasketCheckout.Address)
            .NotNull()
            .WithMessage("Address must not be null.")
            .SetValidator(command => new AddressValidator());
        
        RuleFor(command => command.BasketCheckout.PaymentInfo)
            .NotNull()
            .WithMessage("PaymentInfo must not be null.")
            .SetValidator(command => new PaymentInfoValidator());

        RuleFor(command => command.BasketCheckout.Address)
            .SetValidator(model => new AddressValidator());

    }
}

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
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
        
        RuleFor(command => command.ZipCode)
            .NotNull()
            .WithMessage("ZipCode must not be null.")
            .NotEmpty()
            .WithMessage("ZipCode must not be empty.");
        
        RuleFor(command => command.Street)
            .NotNull()
            .WithMessage("Street must not be null.")
            .NotEmpty()
            .WithMessage("Street must not be empty.");

    }
}

public class PaymentInfoValidator : AbstractValidator<PaymentInfo>
{
    public PaymentInfoValidator()
    {
        RuleFor(command => command.CardNumber)
            .NotNull()
            .WithMessage("CardNumber must not be null.")
            .NotEmpty()
            .WithMessage("CardNumber must not be empty.");
        
        RuleFor(command => command.CardHolderName)
            .NotNull()
            .WithMessage("CardHolderName must not be null.")
            .NotEmpty()
            .WithMessage("CardHolderName must not be empty.");
        
        RuleFor(command => command.CardSecurityNumber)
            .NotNull()
            .WithMessage("CardSecurityNumber must not be null.")
            .NotEmpty()
            .WithMessage("CardSecurityNumber must not be empty.");
        
        RuleFor(command => command.CardExpiration)
            .NotNull()
            .WithMessage("CardExpiration must not be null.");
        
        RuleFor(command => command.CardType)
            .NotNull()
            .WithMessage("CardType must not be null.");

    }
}