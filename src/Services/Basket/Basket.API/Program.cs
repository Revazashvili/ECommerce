using System.Text.Json.Serialization;
using Basket.API.Endpoints;
using Basket.API.IntegrationEvents;
using Basket.API.Interfaces;
using Basket.API.Repositories;
using Basket.API.Services;
using BuildingBlocks.Setup;
using Confluent.Kafka;
using EventBridge.Kafka;
using MessageBus.Nats;
using Microsoft.IdentityModel.Logging;
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

builder.Services.AddKafkaSubscriber(options =>
{
    var kafkaOptions = builder.Configuration.GetSection("kafkaOptions");
    options.BootstrapServers = kafkaOptions["BootstrapServers"];
    options.GroupId = kafkaOptions["GroupId"];
    options.AutoOffsetReset = Enum.Parse<AutoOffsetReset>(kafkaOptions["AutoOffsetReset"]);
    options.EnableAutoCommit = bool.Parse(kafkaOptions["EnableAutoCommit"]);
});

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

app.UseKafkaSubscriber(subscriber =>
{
    subscriber.Subscribe<ProductStockUnAvailableIntegrationEvent, ProductStockUnAvailableIntegrationEventHandler>("ProductStockUnAvailable");
    subscriber.Subscribe<OrderPlacedIntegrationEvent, OrderPlacedIntegrationEventHandler>("OrderPlaced");
});

await app.RunAsync();