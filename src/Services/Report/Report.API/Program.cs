using System.Reflection;
using System.Text.Json.Serialization;
using BuildingBlocks.FluentValidation;
using Confluent.Kafka;
using Elastic.Clients.Elasticsearch;
using EventBridge.Kafka;
using Report.API.Common;
using Report.API.Endpoints;
using Report.API.IntegrationEvents;
using Report.API.Models;
using Report.API.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<EnumSchemaFilter>();   
});

var assembly = Assembly.GetExecutingAssembly();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
builder.Services.AddFluentValidation(assembly);

var elasticsearchClientSettings = new ElasticsearchClientSettings()
    .DefaultMappingFor(typeof(Order), descriptor => descriptor.IndexName("orders"));

builder.Services.AddScoped<ElasticsearchClient>(_ => new ElasticsearchClient(elasticsearchClientSettings));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddKafkaSubscriber(configurator =>
{
    configurator.KafkaOptions = builder.Configuration.GetSection("KafkaOptions").Get<KafkaOptions>();
    configurator.Subscriber = subscriber =>
    {
        subscriber.Subscribe<SetOrderStatusPaidIntegrationEvent, SetOrderStatusPaidIntegrationEventHandler>(
            "OrderStatusSetPaid");
    };
});
builder.Host.UseSerilog((_, configuration) => configuration.WriteTo.Console());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var apiEndpointRouteBuilder = app.MapApi();
apiEndpointRouteBuilder.MapReport();

app.UseFluentValidation();

app.Run();