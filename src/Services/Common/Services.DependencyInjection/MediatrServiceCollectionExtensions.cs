using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Services.DependencyInjection;

public static class MediatrServiceCollectionExtensions
{
    public static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        var callingAssembly = Assembly.GetCallingAssembly();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(callingAssembly));

        return services;
    }
    
    public static IServiceCollection AddMediatrWithValidation(this IServiceCollection services)
    {
        var callingAssembly = Assembly.GetCallingAssembly();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(callingAssembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddValidatorsFromAssembly(callingAssembly);
        
        return services;
    }
}

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