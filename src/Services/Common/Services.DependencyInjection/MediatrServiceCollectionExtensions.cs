using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Services.Common;

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