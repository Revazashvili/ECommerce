using System.Reflection;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using BuildingBlocks.FluentValidation;
using EventBridge.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.Features.OrderPlaced;
using Products.Application.Services;
using Refit;

namespace Products.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IAmazonS3>(provider =>
        {
            var awsCredentials =
                new BasicAWSCredentials(configuration["AWS:ACCESS_KEY"], configuration["AWS:SECRET_KEY"]);
            var amazonS3Config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1
            };
            return new AmazonS3Client(awsCredentials, amazonS3Config);
        });

        services.AddScoped<IImageService, ImageService>();
        
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        services.AddFluentValidation(assembly);

        services.AddKafkaSubscriber(kafkaConfiguration => 
        {
            kafkaConfiguration.WithKafkaOptions(configuration.GetSection("KafkaOptions").Get<KafkaOptions>());
            kafkaConfiguration.RegisterServicesFromAssembly(assembly);
            kafkaConfiguration.ConfigureSubscriber(subscriber =>
            {
                subscriber.Subscribe<OrderPlacedIntegrationEvent, OrderPlacedIntegrationEventHandler>("ordering.OrderPlaced");
            });
        });
        
        services.AddRefitClient<IInventoryService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["InventoryApiUrl"]));

        return services;
    }
}