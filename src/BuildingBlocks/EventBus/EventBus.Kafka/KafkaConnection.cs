using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace EventBus.Kafka;

public class KafkaConnection
{
    private readonly ProducerConfig _producerConfiguration;
    private readonly SchemaRegistryConfig _schemaRegistryConfiguration;
    private readonly ConsumerConfig _consumerConfiguration;
    private object? _producerBuilder;

    public KafkaConnection(ProducerConfig producerConfiguration, ConsumerConfig consumerConfiguration,
        SchemaRegistryConfig schemaRegistryConfiguration)
    {
        _producerConfiguration =producerConfiguration ?? throw new ArgumentNullException(nameof(producerConfiguration));
        _consumerConfiguration =consumerConfiguration ?? throw new ArgumentNullException(nameof(consumerConfiguration));
        _schemaRegistryConfiguration = schemaRegistryConfiguration ?? throw new ArgumentNullException(nameof(schemaRegistryConfiguration));
    }

    public IProducer<Null, string> BuildProducer()
    {
        if (_producerBuilder != null)
            return (IProducer<Null, string>)_producerBuilder;
        
        var schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfiguration);
        _producerBuilder = new ProducerBuilder<Null, string>(_producerConfiguration)
            .Build();

        return (IProducer<Null,string>)_producerBuilder;
    }

    public IConsumer<Null, string> BuildConsumer() 
    {
        var consumer = new ConsumerBuilder<Null, string>(_consumerConfiguration)
            .Build();

        return consumer;
    }
}