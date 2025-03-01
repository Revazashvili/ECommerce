using System.Reflection;
using BuildingBlocks.FluentValidation;
using Confluent.Kafka;
using EventBridge.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.BackgroundServices;
using Ordering.Application.Services;

namespace Ordering.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        services.AddFluentValidation(assembly);

        services.AddKafkaSubscriber(options =>
        {
            var kafkaOptions = configuration.GetSection("kafkaOptions");
            options.BootstrapServers = kafkaOptions["BootstrapServers"];
            options.GroupId = kafkaOptions["GroupId"];
            options.AutoOffsetReset = Enum.Parse<AutoOffsetReset>(kafkaOptions["AutoOffsetReset"]);
            options.EnableAutoCommit = bool.Parse(kafkaOptions["EnableAutoCommit"]);
        });

        services.AddHostedService<OrderProcessingBackgroundService>();
        services.AddScoped<IIdentityService, IdentityService>();
        
        return services;
    }
}