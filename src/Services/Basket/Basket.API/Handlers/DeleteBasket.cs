using Basket.API.Interfaces;
using Basket.API.Services;
using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;

namespace Basket.API.Handlers;

public record DeleteBasketCommand : IValidatedCommand<None>;

public class DeleteBasketCommandHandler : IValidatedCommandHandler<DeleteBasketCommand, None>
{
    private readonly ILogger<UpdateBasketCommand> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IIdentityService _identityService;

    public DeleteBasketCommandHandler(ILogger<UpdateBasketCommand> logger,IBasketRepository basketRepository,
        IIdentityService identityService)
    {
        _logger = logger;
        _basketRepository = basketRepository;
        _identityService = identityService;
    }
    
    public async Task<Either<None, ValidationResult>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _identityService.GetUserId();
            await _basketRepository.DeleteBasketAsync(userId,cancellationToken);
            return None.Instance;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}", nameof(DeleteBasketCommandHandler));
            return new ValidationResult("Can't delete basket");
        }
    }
}