using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.EntityConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders", Schema.Order);
        
        builder.HasKey(o => o.OrderNumber);

        builder.Ignore(b => b.DomainEvents);

        builder.Property(item => item.OrderNumber)
            .IsRequired();
        
        builder.Property(item => item.UserId)
            .IsRequired();
        
        var converter = new ValueConverter<OrderStatus, string>(
            v => v.ToString(),
            v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));

        builder.Property(order => order.OrderStatus)
            .IsRequired()
            .HasConversion(converter);

        builder.Property(order => order.OrderingDate)
            .IsRequired();

        builder.HasMany(order => order.OrderItems);
        
        builder.OwnsOne(o => o.Address);
    }
}