using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Services.Common.Mediator;

namespace Products.Infrastructure;

public class ProductsContextFactory : IDesignTimeDbContextFactory<ProductsContext>
{
    public ProductsContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=products;User Id=postgres;Password=mysecretpassword;");
        optionsBuilder.UseSnakeCaseNamingConvention();
        return new ProductsContext(optionsBuilder.Options, new NoMediator());
    }
}
