using EventBridge.Outbox;

namespace Ordering.Infrastructure.Repositories;

public class OutboxMessageRepository(OrderingContext orderingContext) : IOutboxMessageRepository
{
    public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        await orderingContext.OutboxMessages.AddAsync(message, cancellationToken);
    }
}