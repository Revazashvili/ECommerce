using System.Reflection;
using System.Text.Json.Serialization;
using Basket.API.Endpoints;
using Basket.API.IntegrationEvents;
using Basket.API.Interfaces;
using Basket.API.Repositories;
using Basket.API.Services;
using BuildingBlocks.Swagger;
using EventBridge.Kafka;
using MessageBus.Nats;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
IdentityModelEventSource.ShowPII = true;
// prevent JWT claim keys getting mapped to the XML soap scheme URLs
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

var identitySection = builder.Configuration.GetSection("Identity");

builder.Services.AddEndpointsApiExplorer()
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication().AddJwtBearer(options =>
    {

        options.Authority = identitySection["Url"];
        options.RequireHttpsMetadata = false;
        options.Audience = identitySection["Audience"];
        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddSwagger(builder.Configuration, "Swagger", "Identity")
    .ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .AddScoped<IIdentityService, IdentityService>()
    .AddScoped<IBasketRepository, RedisBasketRepository>()
    .AddKafkaSubscriber(configuration =>
    {
        configuration.WithKafkaOptions(builder.Configuration.GetSection("KafkaOptions").Get<KafkaOptions>());
        configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        configuration.ConfigureSubscriber(subscriber =>
        {
            subscriber.Subscribe<ProductStockUnAvailableIntegrationEvent, ProductStockUnAvailableIntegrationEventHandler>(
                "ProductStockUnAvailable");
            subscriber.Subscribe<OrderPlacedIntegrationEvent, OrderPlacedIntegrationEventHandler>
                ("OrderPlaced");
        });
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

await app.RunAsync();