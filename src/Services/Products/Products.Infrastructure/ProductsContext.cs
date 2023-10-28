using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Infrastructure;

public class ProductsContext : DbContext,IUnitOfWork
{
    
    public ProductsContext(DbContextOptions<ProductsContext> options) : base(options){}
    
    public ProductsContext() { }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public void RejectChanges()
    {
        var entityEntries = ChangeTracker.Entries()
            .Where(e => e.State != EntityState.Unchanged);
        
        foreach (var entry in entityEntries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Modified:
                case EntityState.Deleted:
                    entry.Reload();
                    break;
            }
        }
    }
}