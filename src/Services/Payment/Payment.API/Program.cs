using Confluent.Kafka;
using EventBridge.Kafka;
using EventBridge.Outbox;
using Microsoft.EntityFrameworkCore;
using Payment.API.IntegrationEvents;
using Payment.API.Persistence;
using Payment.API.Persistence.Respositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafkaSubscriber(() => builder.Configuration.GetSection("KafkaOptions").Get<KafkaOptions>());

var connectionString = builder.Configuration.GetConnectionString(nameof(PaymentContext));

builder.Services.AddDbContext<PaymentContext>(builder => builder.UseNpgsql(connectionString,
    optionsBuilder =>
    {
        optionsBuilder.MigrationsAssembly(typeof(PaymentContext).Assembly.FullName);
    }));

builder.Services.AddScoped<IUnitOfWork, PaymentContext>();
builder.Services.AddOutboxDispatcher(provider =>
{
    var paymentContext = provider.GetRequiredService<PaymentContext>();

    return new OutboxMessageRepository(paymentContext);
});

builder.Host.UseSerilog((_, configuration) => configuration.WriteTo.Console());

var app = builder.Build();

app.UseKafkaSubscriber(subscriber =>
{
    subscriber.Subscribe<
            OrderStatusChangedToAvailableQuantityIntegrationEvent,
            OrderStatusChangedToAvailableQuantityIntegrationEventHandler>
        ("OrderStatusChangedToAvailableQuantity");
});

app.Run();