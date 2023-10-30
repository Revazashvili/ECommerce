using EventBus.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.BackgroundServices;
using Services.DependencyInjection;

namespace Ordering.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddMediatrWithValidation();

        services.AddKafka(configuration);

        services.AddHostedService<OrderProcessingBackgroundService>();
        
        return services;
    }
}