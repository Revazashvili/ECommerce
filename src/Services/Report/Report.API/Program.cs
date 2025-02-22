using System.Text.Json.Serialization;
using Confluent.Kafka;
using Elastic.Clients.Elasticsearch;
using EventBridge.Kafka;
using Report.API.Common;
using Report.API.Endpoints;
using Report.API.IntegrationEvents;
using Report.API.Models;
using Report.API.Repositories;
using Services.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<EnumSchemaFilter>();   
});
builder.Host.UseSerilogLogging();
builder.Services.AddMediatrWithValidation();

var elasticsearchClientSettings = new ElasticsearchClientSettings()
    .DefaultMappingFor(typeof(Order), descriptor => descriptor.IndexName("orders"));

builder.Services.AddScoped<ElasticsearchClient>(_ => new ElasticsearchClient(elasticsearchClientSettings));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddKafkaSubscriber(options =>
{
    var kafkaOptions = builder.Configuration.GetSection("kafkaOptions");
    options.BootstrapServers = kafkaOptions["BootstrapServers"];
    options.GroupId = kafkaOptions["GroupId"];
    options.AutoOffsetReset = Enum.Parse<AutoOffsetReset>(kafkaOptions["AutoOffsetReset"]);
    options.EnableAutoCommit = bool.Parse(kafkaOptions["EnableAutoCommit"]);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var apiEndpointRouteBuilder = app.MapApi();
apiEndpointRouteBuilder.MapReport();

app.UseFluentValidationMiddleware();
app.UseKafkaSubscriber(subscriber =>
{
    subscriber.Subscribe<SetOrderStatusPaidIntegrationEvent, SetOrderStatusPaidIntegrationEventHandler>("OrderStatusSetPaid");
});

app.Run();