using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.EntityConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items", Schema.Order);
        
        builder.HasKey(o => o.Id);

        builder.Ignore(b => b.DomainEvents);

        builder.Property(item => item.Id)
            .IsRequired();
        
        builder.Property(item => item.ProductId)
            .IsRequired();
        
        builder.Property(item => item.ProductName)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.Property(item => item.Price)
            .IsRequired()
            .HasPrecision(10,2);
        
        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.Property(item => item.PictureUrl)
            .IsRequired()
            .HasMaxLength(250);
    }
}