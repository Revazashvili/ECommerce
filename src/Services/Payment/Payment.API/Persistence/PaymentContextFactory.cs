using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Payment.API.Persistence;

public class PaymentContextFactory : IDesignTimeDbContextFactory<PaymentContext>
{
    public PaymentContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PaymentContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=payment;User Id=postgres;Password=mysecretpassword;");
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new PaymentContext(optionsBuilder.Options);
    }
}
