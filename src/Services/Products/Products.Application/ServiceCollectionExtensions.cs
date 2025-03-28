using System.Reflection;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using BuildingBlocks.FluentValidation;
using EventBridge.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.IntegrationEvents;
using Products.Application.Services;

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
                subscriber.Subscribe<SetOrderPendingStatusIntegrationEvent, SetOrderPendingStatusIntegrationEventHandler>(
                        "OrderSetOrderPendingStatus");
            });
        });

        return services;
    }
}