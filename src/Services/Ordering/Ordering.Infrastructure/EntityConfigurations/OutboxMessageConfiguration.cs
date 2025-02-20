using EventBridge.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Infrastructure.EntityConfigurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(outboxMessage => outboxMessage.Id);

        builder.Property(outboxMessage => outboxMessage.AggregateId)
            .IsRequired();
        
        builder.Property(outboxMessage => outboxMessage.Topic)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(outboxMessage => outboxMessage.Payload)
            .IsRequired();
        
        builder.Property(outboxMessage => outboxMessage.Timestamp)
            .IsRequired();
    }
}