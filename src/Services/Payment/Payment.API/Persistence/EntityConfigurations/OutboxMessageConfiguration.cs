using EventBridge.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.API.Persistence.EntityConfigurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages", Schema.Outbox);
        builder.HasKey(outboxMessage => outboxMessage.Id);

        builder.Property(outboxMessage => outboxMessage.Id);

        builder.Property(outboxMessage => outboxMessage.AggregateId)
            .IsRequired();

        builder.Property(outboxMessage => outboxMessage.Topic)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(outboxMessage => outboxMessage.Payload)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(outboxMessage => outboxMessage.Timestamp)
            .IsRequired();
    }
}