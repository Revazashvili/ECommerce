using System.Reflection;
using EventBridge.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Services.Common.Domain;

namespace Products.Infrastructure;

public class ProductsContext : DbContext,IUnitOfWork
{
    private readonly IMediator _mediator;

    public ProductsContext(DbContextOptions<ProductsContext> options,IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        var tasks = domainEvents
            .Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken))
            .ToList();
        
        await Task.WhenAll(tasks);

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
        
        return await base.SaveChangesAsync(cancellationToken);
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