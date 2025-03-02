using EventBridge.Subscriber;

namespace EventBridge.Kafka;

public class KafkaSubscriberConfiguration : SubscriberConfiguration
{
    internal KafkaOptions KafkaOptions { get; private set; }

    public void WithKafkaOptions(KafkaOptions kafkaOptions)
    {
        KafkaOptions = kafkaOptions;
    }
}