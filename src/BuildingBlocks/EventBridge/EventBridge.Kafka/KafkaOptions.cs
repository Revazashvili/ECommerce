using Confluent.Kafka;

namespace EventBridge.Kafka;

public class KafkaOptions
{
    public string BootstrapServers { get; set; }
    public string GroupId { get; set; }
    public AutoOffsetReset AutoOffsetReset { get; set; }
    public bool EnableAutoCommit { get; set; }
}