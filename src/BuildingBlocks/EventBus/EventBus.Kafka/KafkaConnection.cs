using Confluent.Kafka;

namespace EventBus.Kafka;

public class KafkaConnection
{
    private readonly ProducerConfig _producerConfiguration;
    private readonly ConsumerConfig _consumerConfiguration;
    private object? _producerBuilder;

    public KafkaConnection(ProducerConfig producerConfiguration, ConsumerConfig consumerConfiguration)
    {
        _producerConfiguration =producerConfiguration ?? throw new ArgumentNullException(nameof(producerConfiguration));
        _consumerConfiguration =consumerConfiguration ?? throw new ArgumentNullException(nameof(consumerConfiguration));
    }

    public IProducer<Null, string> BuildProducer()
    {
        if (_producerBuilder != null)
            return (IProducer<Null, string>)_producerBuilder;
        
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