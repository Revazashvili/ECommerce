using System.Reflection;
using EventBridge.Kafka;
using EventBridge.Outbox;
using Microsoft.EntityFrameworkCore;
using Payment.API.Persistence;
using Payment.API.Persistence.Respositories;
using Payment.API.ProductsReserved;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafkaSubscriber(configuration =>
{
    configuration.WithKafkaOptions(builder.Configuration.GetSection("KafkaOptions").Get<KafkaOptions>());
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    configuration.ConfigureSubscriber(subscriber =>
    {
        subscriber.Subscribe<ProductsReservedIntegrationEvent, ProductsReservedIntegrationEventHandler>("ProductsReserved");
    });
});

var connectionString = builder.Configuration.GetConnectionString(nameof(PaymentContext));

builder.Services.AddDbContext<PaymentContext>(builder =>
{
    builder.UseNpgsql(connectionString,
        optionsBuilder =>
        {
            optionsBuilder.MigrationsAssembly(typeof(PaymentContext).Assembly.FullName);
        });
    builder.UseSnakeCaseNamingConvention();
});

builder.Services.AddScoped<IUnitOfWork, PaymentContext>();
builder.Services.AddOutboxDispatcher(provider =>
{
    var paymentContext = provider.GetRequiredService<PaymentContext>();

    return new OutboxMessageRepository(paymentContext);
});

builder.Host.UseSerilog((_, configuration) => configuration.WriteTo.Console());

var app = builder.Build();

app.Run();