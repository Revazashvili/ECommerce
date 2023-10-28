using Basket.API.Interfaces;
using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;

namespace Basket.API.Handlers;

public record DeleteBasketCommand(int UserId) : IValidatedCommand<None>;

public class DeleteBasketCommandHandler : IValidatedCommandHandler<DeleteBasketCommand, None>
{
    private readonly ILogger<UpdateBasketCommand> _logger;
    private readonly IBasketRepository _basketRepository;

    public DeleteBasketCommandHandler(ILogger<UpdateBasketCommand> logger,IBasketRepository basketRepository)
    {
        _logger = logger;
        _basketRepository = basketRepository;
    }
    
    public async Task<Either<None, ValidationResult>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _basketRepository.DeleteBasketAsync(request.UserId.ToString(),cancellationToken);
            return None.Instance;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}", nameof(DeleteBasketCommandHandler));
            return new ValidationResult("Can't delete basket");
        }
    }
}

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotNull()
            .WithMessage("UserId must not be null.")
            .NotEqual(0)
            .WithMessage("Id must not equal to zero.");
    }
}