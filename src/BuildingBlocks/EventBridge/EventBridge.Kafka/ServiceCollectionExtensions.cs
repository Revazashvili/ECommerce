using EventBridge.Subscriber;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBridge.Kafka;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaSubscriber(this IServiceCollection services, 
        Action<KafkaSubscriberConfigurator> kafkaSubscriberConfigurator)
    {
        var configurator = new KafkaSubscriberConfigurator();
        kafkaSubscriberConfigurator(configurator);

        services.AddSingleton(configurator.KafkaOptions);
        services.AddEventBridge<KafkaIntegrationEventSubscriberService>(configurator);

        return services;
    }
}