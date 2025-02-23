using System.Reflection;
using EventBridge.Outbox;
using Microsoft.EntityFrameworkCore;
using Services.Common.Domain;

namespace Payment.API.Persistence;

public class PaymentContext : DbContext, IUnitOfWork
{
    public PaymentContext(DbContextOptions<PaymentContext> options) : base(options) { }
    
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
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