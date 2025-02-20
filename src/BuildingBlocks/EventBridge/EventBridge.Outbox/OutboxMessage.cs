namespace EventBridge.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string AggregateId { get; set; }
    public string Topic { get; set; }
    public string Payload { get; set; }
    public DateTime Timestamp { get; set; }
}