using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EventBridge.Kafka;

public class KafkaIntegrationEventSubscriber : IIntegrationEventSubscriber
{
    private readonly InMemorySubscriptionOptionsManager _subscriptionOptionsManager;

    internal KafkaIntegrationEventSubscriber(InMemorySubscriptionOptionsManager subscriptionOptionsManager)
    {
        _subscriptionOptionsManager = subscriptionOptionsManager;
    }
    
    public void Subscribe<TEvent, TEventHandler>(string topic) 
        where TEvent : IntegrationEvent 
        where TEventHandler : IIntegrationEventHandler<TEvent>
    {
        ProcessEvent processFunc = (consumeResult, serviceProvider, cancellationToken) =>
        {
            var message = (TEvent)JsonConvert.DeserializeObject(consumeResult.Message!.Value, typeof(TEvent));

            var handler = serviceProvider.GetRequiredService<TEventHandler>();

            return handler.HandleAsync(message, cancellationToken);
        };
        
        _subscriptionOptionsManager.AddProcessEvent(topic, processFunc);
    }
}