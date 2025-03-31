using System.Reflection;
using BuildingBlocks.FluentValidation;
using EventBridge.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Features.PaymentFailed;
using Ordering.Application.Features.PaymentSucceeded;
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

        services.AddKafkaSubscriber(subscriberConfiguration =>
        {
            subscriberConfiguration.WithKafkaOptions(configuration.GetSection("KafkaOptions").Get<KafkaOptions>());
            subscriberConfiguration.RegisterServicesFromAssembly(assembly);
            subscriberConfiguration.ConfigureSubscriber(subscriber =>
            {
                subscriber.Subscribe<OrderQuantityNotAvailableIntegrationEvent, OrderQuantityNotAvailableIntegrationEventHandler>
                        ("OrderQuantityNotAvailable");
                subscriber.Subscribe<OrderQuantityAvailableIntegrationEvent, OrderQuantityAvailableIntegrationEventHandler>(
                        "OrderQuantityAvailable");
                subscriber.Subscribe<OrderPaymentSucceededIntegrationEvent, OrderPaymentSucceededIntegrationEventHandler>(
                        "OrderPaymentSucceeded");
                subscriber.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>(
                    "OrderPaymentFailed");
            });
        });

        services.AddScoped<IIdentityService, IdentityService>();
        
        return services;
    }
}