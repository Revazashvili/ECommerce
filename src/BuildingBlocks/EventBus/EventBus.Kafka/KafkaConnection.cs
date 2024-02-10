using Confluent.Kafka;

namespace EventBus.Kafka;

public class KafkaConnection(ProducerConfig producerConfiguration,
    ConsumerConfig consumerConfiguration)
{
    private IProducer<Null, string>? _producerBuilder;
    private IConsumer<Null, string>? _consumerBuilder;

    /// <summary>
    /// Builds <see cref="IProducer{TKey,TValue}"/> based of injected <see cref="ProducerConfig"/>.
    /// </summary>
    /// <returns>Instance of <see cref="IProducer{TKey,TValue}"/>.</returns>
    public IProducer<Null, string> BuildProducer()
    {
        if (_producerBuilder is not null)
            return _producerBuilder;
        
        _producerBuilder = new ProducerBuilder<Null, string>(producerConfiguration)
            .Build();

        return _producerBuilder;
    }

    /// <summary>
    /// Builds <see cref="IConsumer{TKey,TValue}"/> based of injected <see cref="ConsumerConfig"/>.
    /// </summary>
    /// <returns>Instance of <see cref="IConsumer{TKey,TValue}"/>.</returns>

    public IConsumer<Null, string> BuildConsumer()
    {
        if (_consumerBuilder is not null)
            return _consumerBuilder;
        
        _consumerBuilder = new ConsumerBuilder<Null, string>(consumerConfiguration)
            .Build();

        return _consumerBuilder;
    }
}