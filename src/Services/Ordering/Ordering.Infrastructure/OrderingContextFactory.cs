using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Services.Common.Mediator;

namespace Ordering.Infrastructure;

public class OrderingContextFactory : IDesignTimeDbContextFactory<OrderingContext>
{
    public OrderingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrderingContext>();
        optionsBuilder
            .UseSqlServer(
            "Server=localhost;Database=OrderingDB;User Id=SA;Password=myStrong(!)Password;TrustServerCertificate=true;");

        return new OrderingContext(optionsBuilder.Options, new NoMediator());
    }
}
