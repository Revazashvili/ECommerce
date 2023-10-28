using Basket.API.Interfaces;
using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;

namespace Basket.API.Handlers;

public record GetBasketQuery(int UserId) : IValidatedQuery<Models.Basket>;

public class GetBasketQueryHandler : IValidatedQueryHandler<GetBasketQuery, Models.Basket>
{
    private readonly ILogger<GetBasketQueryHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private static readonly ValidationResult Error = new("Can't find basket for user");

    public GetBasketQueryHandler(ILogger<GetBasketQueryHandler> logger,IBasketRepository basketRepository)
    {
        _logger = logger;
        _basketRepository = basketRepository;
    }
    
    public async Task<Either<Models.Basket, ValidationResult>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var basket = await _basketRepository.GetBasketAsync(request.UserId.ToString(), cancellationToken);
            return basket is null ? Error : basket;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {Handler}",nameof(GetBasketQueryHandler));
            return Error;
        }
    }
}

public class GetBasketQueryValidator : AbstractValidator<GetBasketQuery>
{
    public GetBasketQueryValidator()
    {
        RuleFor(command => command.UserId)
            .NotNull()
            .WithMessage("UserId must not be null.")
            .NotEqual(0)
            .WithMessage("Id must not equal to zero.");
    }
}