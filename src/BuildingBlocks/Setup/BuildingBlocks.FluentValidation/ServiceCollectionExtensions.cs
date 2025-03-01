using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.FluentValidation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddValidatorsFromAssemblies(assemblies);
        
        return services;
    }
}