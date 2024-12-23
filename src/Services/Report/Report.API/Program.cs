using System.Text.Json.Serialization;
using Elastic.Clients.Elasticsearch;
using EventBus;
using EventBus.Kafka;
using Report.API.Common;
using Report.API.Endpoints;
using Report.API.IntegrationEvents.Events;
using Report.API.IntegrationEvents.Handlers;
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

builder.Services.AddKafka(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var apiEndpointRouteBuilder = app.MapApi();
apiEndpointRouteBuilder.MapReport();

app.UseFluentValidationMiddleware();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.SubscribeAsync<SetOrderStatusPaidIntegrationEvent, SetOrderStatusPaidIntegrationEventHandler>();

app.Run();