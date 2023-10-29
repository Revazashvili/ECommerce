using System.Text.Json.Serialization;
using Basket.API.Endpoints;
using Basket.API.IntegrationEvents.EventHandlers;
using Basket.API.IntegrationEvents.Events;
using Basket.API.Interfaces;
using Basket.API.Repositories;
using EventBus;
using EventBus.Kafka;
using Services.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatrWithValidation();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<IBasketRepository, RedisBasketRepository>();
builder.Services.AddScoped<ProductStockUnAvailableIntegrationEventHandler>();

builder.Services.AddRedis(builder.Configuration);
builder.Services.AddKafka(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var apiEndpointRouteBuilder = app.MapApi();
apiEndpointRouteBuilder.MapBasket();

app.UseFluentValidationMiddleware();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.SubscribeAsync<ProductStockUnAvailableIntegrationEvent, ProductStockUnAvailableIntegrationEventHandler>();

await app.RunAsync();