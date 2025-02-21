using Newtonsoft.Json;

namespace EventBridge.Outbox;

public class OutboxIntegrationEventDispatcher(IOutboxMessageRepository outboxMessageRepository) : IIntegrationEventDispatcher
{
    public Task DispatchAsync(string topic, IntegrationEvent @event, CancellationToken cancellationToken)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = @event.Id,
            AggregateId = @event.AggregateId,
            Topic = topic,
            Payload = JsonConvert.SerializeObject(@event),
            Timestamp = @event.Timestamp
        };

        return outboxMessageRepository.AddAsync(outboxMessage, cancellationToken);
    }
}