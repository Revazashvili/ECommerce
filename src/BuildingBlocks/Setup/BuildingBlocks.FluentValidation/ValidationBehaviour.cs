using FluentValidation;
using MediatR;

namespace BuildingBlocks.FluentValidation;

internal class ValidationBehaviour<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();
        
        var context = new ValidationContext<TRequest>(request);

        var validationTasks = validators.Select(v => v.ValidateAsync(context, cancellationToken));
        
        var validationResults = await Task.WhenAll(validationTasks);
        
        var validationFailures = validationResults
            .SelectMany(r => r.Errors)
            .ToList();

        if (validationFailures.Count != 0)
            throw new ValidationException(validationFailures);
        
        return await next();
    }
}