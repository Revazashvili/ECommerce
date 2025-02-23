using EventBridge.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.API.Persistence.EntityConfigurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages", Schema.Outbox);
        builder.HasKey(outboxMessage => outboxMessage.Id)
            .HasName("id");

        builder.Property(outboxMessage => outboxMessage.AggregateId)
            .IsRequired()
            .HasColumnName("aggregate_id");
        
        builder.Property(outboxMessage => outboxMessage.Topic)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("topic");
        
        builder.Property(outboxMessage => outboxMessage.Payload)
            .IsRequired()
            .HasColumnName("payload");
        
        builder.Property(outboxMessage => outboxMessage.Timestamp)
            .IsRequired()
            .HasColumnName("timestamp");
    }
}