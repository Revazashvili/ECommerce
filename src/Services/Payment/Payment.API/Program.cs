using EventBus;
using EventBus.Kafka;
using Payment.API.IntegrationEvents.EventHandlers;
using Payment.API.IntegrationEvents.Events;
using Services.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafka(builder.Configuration);
builder.Services.AddScoped<OrderStatusChangedToAvailableQuantityIntegrationEventHandler>();
builder.Host.UseSerilogLogging();

var app = builder.Build();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus
    .SubscribeAsync<OrderStatusChangedToAvailableQuantityIntegrationEvent,
        OrderStatusChangedToAvailableQuantityIntegrationEventHandler>();

app.Run();