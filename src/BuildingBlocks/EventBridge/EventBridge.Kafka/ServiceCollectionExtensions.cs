using EventBridge.Subscriber;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBridge.Kafka;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaSubscriber(this IServiceCollection services, Action<KafkaOptions> configureOptions)
    {
        var options = new KafkaOptions();
        configureOptions(options);
        
        services.AddSingleton(options);
        services.AddEventBridge<KafkaIntegrationEventSubscriberService>();

        return services;
    }

    public static void UseKafkaSubscriber(this IHost application,
        Action<IIntegrationEventSubscriber> subscriberAction)
    {
        var integrationEventSubscriber = application.Services.GetRequiredService<IIntegrationEventSubscriber>();
        
        subscriberAction(integrationEventSubscriber);
    }
    
}