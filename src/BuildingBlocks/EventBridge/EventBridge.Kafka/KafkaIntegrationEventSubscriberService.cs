using Confluent.Kafka;
using EventBridge.Subscriber;

namespace EventBridge.Kafka;

public class KafkaIntegrationEventSubscriberService : IntegrationEventSubscriberService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConsumerConfig _consumerConfig;
    public KafkaIntegrationEventSubscriberService(KafkaOptions kafkaOptions, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaOptions.BootstrapServers,
            GroupId = kafkaOptions.GroupId,
            AutoOffsetReset = kafkaOptions.AutoOffsetReset,
            EnableAutoCommit = kafkaOptions.EnableAutoCommit,
            AllowAutoCreateTopics = false
        };
    }

    protected override Task Subscribe(string topic, ProcessEvent processEventFunction, CancellationToken cancellationToken)
    {
        var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
            .Build();
        
        consumer.Subscribe(topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            var consumerResult = consumer.Consume(cancellationToken);

            if (consumerResult.Message == null || consumerResult.IsPartitionEOF)
                continue;
            
            processEventFunction(consumerResult.Message.Value, _serviceProvider, cancellationToken);
        }
        
        Console.WriteLine("cancellation requested");
        return Task.CompletedTask;
    }
}