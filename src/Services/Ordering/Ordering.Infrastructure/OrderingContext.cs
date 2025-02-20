using System.Reflection;
using EventBridge.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Services.Common.Domain;
using Services.Common.Extensions;

namespace Ordering.Infrastructure;

public class OrderingContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public OrderingContext(DbContextOptions<OrderingContext> options,IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
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