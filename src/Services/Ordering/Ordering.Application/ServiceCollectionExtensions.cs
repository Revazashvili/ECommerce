using Confluent.Kafka;
using EventBridge.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.BackgroundServices;
using Ordering.Application.Services;
using Services.DependencyInjection;

namespace Ordering.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddMediatrWithValidation();

        services.AddKafkaSubscriber(options =>
        {
            var kafkaOptions = configuration.GetSection("kafkaOptions");
            options.BootstrapServers = kafkaOptions["BootstrapServers"];
            options.GroupId = kafkaOptions["GroupId"];
            options.AutoOffsetReset = (AutoOffsetReset)Enum.Parse(typeof(AutoOffsetReset), kafkaOptions["AutoOffsetReset"]);
            options.EnableAutoCommit = bool.Parse(kafkaOptions["EnableAutoCommit"]);
        });

        services.AddHostedService<OrderProcessingBackgroundService>();
        services.AddScoped<IIdentityService, IdentityService>();
        
        return services;
    }
}