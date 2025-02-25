using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Services.Common.Mediator;

namespace Ordering.Infrastructure;

public class OrderingContextFactory : IDesignTimeDbContextFactory<OrderingContext>
{
    public OrderingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrderingContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ordering;User Id=postgres;Password=mysecretpassword;");
        optionsBuilder.UseSnakeCaseNamingConvention();
        return new OrderingContext(optionsBuilder.Options, new NoMediator());
    }
}
