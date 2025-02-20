using Newtonsoft.Json;

namespace EventBridge.Outbox;

public class OutboxIntegrationEventDispatcher(IOutboxMessageRepository outboxMessageRepository) : IIntegrationEventDispatcher
{
    public Task DispatchAsync<TKey>(IntegrationEvent<TKey> @event, CancellationToken cancellationToken)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = @event.Id,
            AggregateId = @event.AggregateId.ToString(),
            Topic = @event.Topic,
            Payload = JsonConvert.SerializeObject(@event),
            Timestamp = @event.Timestamp
        };

        return outboxMessageRepository.AddAsync(outboxMessage, cancellationToken);
    }
}