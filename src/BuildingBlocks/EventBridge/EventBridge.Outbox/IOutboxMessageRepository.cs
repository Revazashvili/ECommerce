namespace EventBridge.Outbox;

public interface IOutboxMessageRepository
{
    Task AddAsync(OutboxMessage message, CancellationToken cancellationToken);
}