using BlobHelper;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.Services;
using Services.DependencyInjection;

namespace Products.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        
        var settings = new AwsSettings(
            "AKIATRGB7XIRXBMVQAM2",
            "M/roQv4zJCBNPa2Z8a9cBFWlsuSi7P94VUL36SxK",
            AwsRegion.USEast1,
            "ecommerce-microservices");
        
        services.AddScoped<BlobClient>(provider => new BlobClient(settings));
        services.AddScoped<IImageService, ImageService>();
        
        services.AddMediatrWithValidation();
        return services;
    }
}