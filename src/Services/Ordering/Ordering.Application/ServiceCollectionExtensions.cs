using System.Reflection;
using BuildingBlocks.FluentValidation;
using Confluent.Kafka;
using EventBridge.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.BackgroundServices;
using Ordering.Application.IntegrationEvents;
using Ordering.Application.Services;

namespace Ordering.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        services.AddFluentValidation(assembly);

        services.AddKafkaSubscriber(configurator =>
        {
            configurator.KafkaOptions = configuration.GetSection("KafkaOptions").Get<KafkaOptions>();
            configurator.Subscriber = subscriber =>
            {
                subscriber
                    .Subscribe<OrderQuantityNotAvailableIntegrationEvent, OrderQuantityNotAvailableIntegrationEventHandler>
                        ("OrderQuantityNotAvailable");
                subscriber
                    .Subscribe<OrderQuantityAvailableIntegrationEvent, OrderQuantityAvailableIntegrationEventHandler>(
                        "OrderQuantityAvailable");
                subscriber
                    .Subscribe<OrderPaymentSucceededIntegrationEvent, OrderPaymentSucceededIntegrationEventHandler>(
                        "OrderPaymentSucceeded");
                subscriber.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>(
                    "OrderPaymentFailed");
            };
        });

        services.AddHostedService<OrderProcessingBackgroundService>();
        services.AddScoped<IIdentityService, IdentityService>();
        
        return services;
    }
}