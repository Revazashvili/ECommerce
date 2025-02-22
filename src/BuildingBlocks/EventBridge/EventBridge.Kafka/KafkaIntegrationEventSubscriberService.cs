using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace EventBridge.Kafka;

internal class KafkaIntegrationEventSubscriberService : IHostedService
{
    private readonly InMemorySubscriptionOptionsManager _subscriptionOptionsManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConsumerConfig _consumerConfig;

    internal KafkaIntegrationEventSubscriberService(InMemorySubscriptionOptionsManager subscriptionOptionsManager,
        IServiceProvider serviceProvider, KafkaOptions kafkaOptions)
    {
        _subscriptionOptionsManager = subscriptionOptionsManager;
        _serviceProvider = serviceProvider;
        _consumerConfig = new ConsumerConfig
        {
            GroupId = kafkaOptions.GroupId,
            BootstrapServers = kafkaOptions.BootstrapServers,
            AutoOffsetReset = kafkaOptions.AutoOffsetReset,
            EnableAutoCommit = kafkaOptions.EnableAutoCommit,
            AllowAutoCreateTopics = false
        };
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var processEvents = _subscriptionOptionsManager.GetProcessEvents();

        var tasks = processEvents.Select(pair => Subscribe(pair.Key, pair.Value, cancellationToken)).ToList();
        
        await Task.WhenAll(tasks);
    }
    
    private async Task Subscribe(string topic, ProcessEvent func, CancellationToken cancellationToken)
    {
        var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
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