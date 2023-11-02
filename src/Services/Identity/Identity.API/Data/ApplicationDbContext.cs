using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Identity.API.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>()
            .Property(user => user.PersonalNumber)
            .IsRequired()
            .HasMaxLength(11);
        
        var converter = new ValueConverter<ApplicationUserStatus, string>(
            v => v.ToString(),
            v => (ApplicationUserStatus)Enum.Parse(typeof(ApplicationUserStatus), v));

        builder.Entity<ApplicationUser>()
            .Property(order => order.Status)
            .IsRequired()
            .HasConversion(converter);
        
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}