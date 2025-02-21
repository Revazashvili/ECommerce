using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventBridge.Kafka;

public static class ServiceCollectionExtensions
{
    public static void AddKafkaSubscriber(this IServiceCollection services, Action<KafkaOptions> configureOptions)
    {
        var options = new KafkaOptions();
        configureOptions(options);
        
        services.AddSingleton(options);
        services.AddSingleton<InMemorySubscriptionOptionsManager>();

        var handlers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.BaseType == typeof(IIntegrationEventHandler<>))
            .ToList();
        
        foreach (var handler in handlers)
            services.AddTransient(handler);

        services.AddHostedService<KafkaIntegrationEventSubscriberService>();
    }

    public static void UseKafkaSubscriber(this WebApplication application,
        Action<IIntegrationEventSubscriber> subscriberAction)
    {
        var inMemorySubscriptionOptionsManager = application.Services.GetRequiredService<InMemorySubscriptionOptionsManager>();
        
        var subscriber = new KafkaIntegrationEventSubscriber(inMemorySubscriptionOptionsManager);
        
        subscriberAction(subscriber);
    }
    
}