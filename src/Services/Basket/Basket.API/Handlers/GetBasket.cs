using Basket.API.Interfaces;
using Basket.API.Services;
using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;

namespace Basket.API.Handlers;

public record GetBasketQuery : IValidatedQuery<Models.Basket>;

public class GetBasketQueryHandler : IValidatedQueryHandler<GetBasketQuery, Models.Basket>
{
    private readonly ILogger<GetBasketQueryHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IIdentityService _identityService;
    private static readonly ValidationResult Error = new("Can't find basket for user");

    public GetBasketQueryHandler(ILogger<GetBasketQueryHandler> logger,IBasketRepository basketRepository,
        IIdentityService identityService)
    {
        _logger = logger;
        _basketRepository = basketRepository;
        _identityService = identityService;
    }
    
    public async Task<Either<Models.Basket, ValidationResult>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _identityService.GetUserId();
            var basket = await _basketRepository.GetBasketAsync(userId, cancellationToken);
            return basket is null ? Error : basket;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {Handler}",nameof(GetBasketQueryHandler));
            return Error;
        }
    }
}