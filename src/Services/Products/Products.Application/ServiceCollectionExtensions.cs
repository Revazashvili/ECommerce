using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using EventBus.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.IntegrationEvents;
using Products.Application.Services;
using Services.DependencyInjection;

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
        
        services.AddMediatrWithValidation();

        services.AddKafka(configuration);

        services.AddScoped<SetOrderPendingStatusIntegrationEventHandler>();
        return services;
    }
}