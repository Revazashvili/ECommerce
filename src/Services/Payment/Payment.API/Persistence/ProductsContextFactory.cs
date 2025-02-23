using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Services.Common.Mediator;

namespace Payment.API.Persistence;

public class ProductsContextFactory : IDesignTimeDbContextFactory<PaymentContext>
{
    public PaymentContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PaymentContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=payment;User Id=postgres;Password=mysecretpassword;");

        return new PaymentContext(optionsBuilder.Options);
    }
}
