using EventBus.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.BackgroundServices;
using Ordering.Application.IntegrationEvents.EventHandlers;
using Ordering.Application.Services;
using Services.DependencyInjection;

namespace Ordering.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddMediatrWithValidation();

        services.AddKafka(configuration);

        services.AddHostedService<OrderProcessingBackgroundService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<OrderQuantityNotAvailableIntegrationEventHandler>();
        services.AddScoped<OrderQuantityAvailableIntegrationEventHandler>();
        services.AddScoped<OrderPaymentSucceededIntegrationEventHandler>();
        services.AddScoped<OrderPaymentFailedIntegrationEventHandler>();
        
        return services;
    }
}