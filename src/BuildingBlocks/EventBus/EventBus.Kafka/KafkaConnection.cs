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

    public IProducer<Null, T> BuildProducer<T>() 
        where T : class
    {
        if (_producerBuilder != null)
            return (IProducer<Null, T>)_producerBuilder;
        
        var schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfiguration);
        _producerBuilder = new ProducerBuilder<Null, T>(_producerConfiguration)
            .SetValueSerializer(new AvroSerializer<T>(schemaRegistry))
            .Build();

        return (IProducer<Null,T>)_producerBuilder;
    }

    public IConsumer<Null, T> BuildConsumer<T>() 
        where T : class
    {
        var consumer = new ConsumerBuilder<Null, T>(_consumerConfiguration)
            .SetValueDeserializer(new JsonDeserializer<T>().AsSyncOverAsync())
            .Build();

        return consumer;
    }
}