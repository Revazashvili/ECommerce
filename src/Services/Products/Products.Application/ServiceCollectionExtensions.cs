using Microsoft.Extensions.DependencyInjection;
using Services.DependencyInjection;

namespace Products.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatrWithValidation();

        return services;
    }
}