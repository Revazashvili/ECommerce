using System.Text.Json.Serialization;
using Basket.API.Endpoints;
using Basket.API.IntegrationEvents;
using Basket.API.Interfaces;
using Basket.API.Repositories;
using Basket.API.Services;
using BuildingBlocks.Setup;
using BuildingBlocks.Swagger;
using Confluent.Kafka;
using EventBridge.Kafka;
using MessageBus.Nats;
using Microsoft.IdentityModel.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
IdentityModelEventSource.ShowPII = true;

builder.Services.AddEndpointsApiExplorer()
    .AddAuthorization()
    .AddAuthentication(builder.Configuration)
    .AddSwagger(builder.Configuration, "Swagger", "Identity")
    .ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .AddScoped<IIdentityService, IdentityService>()
    .AddScoped<IBasketRepository, RedisBasketRepository>()
    .AddKafkaSubscriber(options =>
    {
        var kafkaOptions = builder.Configuration.GetSection("kafkaOptions");
        options.BootstrapServers = kafkaOptions["BootstrapServers"];
        options.GroupId = kafkaOptions["GroupId"];
        options.AutoOffsetReset = Enum.Parse<AutoOffsetReset>(kafkaOptions["AutoOffsetReset"]);
        options.EnableAutoCommit = bool.Parse(kafkaOptions["EnableAutoCommit"]);
    })
    .AddNatsMessageBus(builder.Configuration);

builder.Host.UseSerilog((_, configuration) => configuration.WriteTo.Console())
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.AddMemoryGrainStorage("baskets");
    });

var app = builder.Build();

app.UseSwagger(builder.Configuration, "Swagger")
    .UseAuthentication()
    .UseAuthorization();

var apiEndpointRouteBuilder = app.MapApi();
apiEndpointRouteBuilder.MapBasket();

app.UseKafkaSubscriber(subscriber =>
{
    subscriber.Subscribe<ProductStockUnAvailableIntegrationEvent, ProductStockUnAvailableIntegrationEventHandler>("ProductStockUnAvailable");
    subscriber.Subscribe<OrderPlacedIntegrationEvent, OrderPlacedIntegrationEventHandler>("OrderPlaced");
});

await app.RunAsync();