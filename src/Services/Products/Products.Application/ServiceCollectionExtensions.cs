using BlobHelper;
using EventBus.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.IntegrationEvents.EventHandlers;
using Products.Application.Services;
using Services.DependencyInjection;

namespace Products.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {
        
        var settings = new AwsSettings(
            configuration["AWS:ACCESS_KEY"],
            configuration["AWS:SECRET_KEY"],
            AwsRegion.USEast1,
            "ecommerce-microservices");
        
        services.AddScoped<BlobClient>(provider => new BlobClient(settings));
        services.AddScoped<IImageService, ImageService>();
        
        services.AddMediatrWithValidation();

        services.AddKafka(configuration);

        services.AddScoped<SetOrderPendingStatusIntegrationEventHandler>();
        return services;
    }
}