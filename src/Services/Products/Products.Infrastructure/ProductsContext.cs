using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Services.Common.Domain;
using Services.Common.Extensions;

namespace Products.Infrastructure;

public class ProductsContext : DbContext,IUnitOfWork
{
    private readonly IMediator _mediator;

    public ProductsContext(DbContextOptions<ProductsContext> options,IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    
    public ProductsContext() { }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var affectedRows = await base.SaveChangesAsync(cancellationToken);

        await _mediator.PublishDomainEventsAsync(this);
        
        return affectedRows;
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