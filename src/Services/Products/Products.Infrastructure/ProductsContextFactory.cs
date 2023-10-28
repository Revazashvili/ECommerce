using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Products.Infrastructure;

public class ProductsContextFactory : IDesignTimeDbContextFactory<ProductsContext>
{
    public ProductsContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Products;User Id=postgres;Password=mysecretpassword;");

        return new ProductsContext(optionsBuilder.Options);
    }
}