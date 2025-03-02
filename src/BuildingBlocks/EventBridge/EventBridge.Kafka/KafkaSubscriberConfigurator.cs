using EventBridge.Subscriber;

namespace EventBridge.Kafka;

public class KafkaSubscriberConfigurator : EventBridgeSubscriberConfiguration
{
    public KafkaOptions KafkaOptions { internal get; set; }
}