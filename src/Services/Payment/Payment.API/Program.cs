using EventBus;
using EventBus.Kafka;
using Payment.API.IntegrationEvents.EventHandlers;
using Payment.API.IntegrationEvents.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafka(builder.Configuration);
builder.Services.AddScoped<OrderStatusChangedToAvailableQuantityIntegrationEventHandler>();

var app = builder.Build();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus
    .SubscribeAsync<OrderStatusChangedToAvailableQuantityIntegrationEvent,
        OrderStatusChangedToAvailableQuantityIntegrationEventHandler>();

app.Run();