using EventBridge.Outbox;

namespace Payment.API.Persistence.Respositories;

public class OutboxMessageRepository : IOutboxMessageRepository
{
    private readonly PaymentContext _context;

    public OutboxMessageRepository(PaymentContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        await _context.OutboxMessages.AddAsync(message, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}