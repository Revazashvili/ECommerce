using FluentValidation;
using MediatR;

namespace Services.Common;

public class ValidationBehaviour<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();
        
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var validationFailures = validationResults
            .SelectMany(r => r.Errors)
            .ToList();

        if (validationFailures.Any())
            throw new ValidationException(validationFailures);
        
        return await next();
    }
}