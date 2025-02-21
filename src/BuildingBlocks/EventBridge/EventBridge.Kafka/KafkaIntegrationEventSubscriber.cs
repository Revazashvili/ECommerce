using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace EventBridge.Kafka;

public class KafkaOptions;

internal class SubscriptionOptions
{
    public string Topic { get; set; }
    public Type EventType { get; set; }
    public Type EventHandlerType { get; set; }
}

internal class InMemorySubscriptionOptionsManager
{
    private readonly Dictionary<string, ProcessEvent> _processEvents = [];
    internal void AddProcessEvent(string topic, ProcessEvent processEvent)
    {
        
        if (_processEvents.Any(options => options.Key == topic))
            return;
        
        _processEvents[topic] = processEvent;
    }
    
    public Dictionary<string, ProcessEvent> GetProcessEvents() => _processEvents;
}

internal delegate Task ProcessEvent(ConsumeResult<string,string> consumeResult, IServiceProvider serviceProvider, CancellationToken cancellationToken);

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

internal class KafkaIntegrationEventSubscriberService : IHostedService
{
    private readonly InMemorySubscriptionOptionsManager _subscriptionOptionsManager;
    private readonly IServiceProvider _serviceProvider;

    internal KafkaIntegrationEventSubscriberService(InMemorySubscriptionOptionsManager subscriptionOptionsManager,
        IServiceProvider serviceProvider)
    {
        _subscriptionOptionsManager = subscriptionOptionsManager;
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var processEvents = _subscriptionOptionsManager.GetProcessEvents();

        var tasks = processEvents.Select((pair) => Subscribe(pair.Key, pair.Value, cancellationToken)).ToList();
        
        await Task.WhenAll(tasks);
    }
    
    private async Task Subscribe(string topic, ProcessEvent func, CancellationToken cancellationToken)
    {
        var consumer = new ConsumerBuilder<string, string>(null) // TODO: pass configuration
            .Build();
        
        consumer.Subscribe(topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            var consumerResult = consumer.Consume();

            if (consumerResult.Message == null || consumerResult.IsPartitionEOF)
                continue;
            
            func(consumerResult, _serviceProvider, cancellationToken);
        }
        
        Console.WriteLine("cancellation requested");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}