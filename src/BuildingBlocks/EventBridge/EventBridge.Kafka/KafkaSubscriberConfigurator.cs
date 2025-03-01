using EventBridge.Subscriber;

namespace EventBridge.Kafka;

public class KafkaSubscriberConfigurator
{
    public KafkaOptions KafkaOptions { internal get; set; }
    public Action<IIntegrationEventSubscriber> Subscriber { internal get; set; }
}