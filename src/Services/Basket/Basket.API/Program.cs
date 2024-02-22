using System.Text.Json.Serialization;
using Basket.API.Endpoints;
using Basket.API.IntegrationEvents.EventHandlers;
using Basket.API.IntegrationEvents.Events;
using Basket.API.Interfaces;
using Basket.API.Repositories;
using Basket.API.Services;
using EventBus;
using EventBus.Kafka;
using MessageBus.Nats;
using Microsoft.IdentityModel.Logging;
using Services.DependencyInjection;
using IIdentityService = Basket.API.Services.IIdentityService;

var builder = WebApplication.CreateBuilder(args);
IdentityModelEventSource.ShowPII = true;

builder.Services.AddEndpointsApiExplorer()
    .AddAuthorization()
    .AddAuthentication(builder.Configuration);

builder.Services.AddSwagger(builder.Configuration);

builder.Host.UseSerilogLogging();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IBasketRepository, RedisBasketRepository>();
builder.Services.AddScoped<ProductStockUnAvailableIntegrationEventHandler>();
builder.Services.AddScoped<OrderPlaceStartedIntegrationEventHandler>();

builder.Services.AddKafka(builder.Configuration);
builder.Services.AddNatsMessageBus(builder.Configuration);

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddMemoryGrainStorage("baskets");
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

var apiEndpointRouteBuilder = app.MapApi();
apiEndpointRouteBuilder.MapBasket();

app.UseSwagger(builder.Configuration);

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.SubscribeAsync<ProductStockUnAvailableIntegrationEvent, ProductStockUnAvailableIntegrationEventHandler>();
eventBus.SubscribeAsync<OrderPlaceStartedIntegrationEvent, OrderPlaceStartedIntegrationEventHandler>();

await app.RunAsync();