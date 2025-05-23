using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Entities;

namespace Products.Infrastructure.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products", Schema.Products);
        
        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(250);

        builder.Property(product => product.Price)
            .IsRequired();
        
        builder.Property(product => product.ImageUrl)
            .IsRequired()
            .HasMaxLength(250);

        builder.HasMany(product => product.Categories)
            .WithMany(category => category.Products)
            .UsingEntity(pc => pc.ToTable("product_categories", Schema.Products));

        builder.HasIndex(product => product.Name);

        builder.Ignore(product => product.DomainEvents);
    }
}