using System.Text.Json.Serialization;
using Products.API.Endpoints;
using Products.Application;
using Products.Infrastructure;
using Services.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var endpointRouteBuilder = app.MapApi();
endpointRouteBuilder.MapProductCategory();
endpointRouteBuilder.MapProduct();

app.UseFluentValidationMiddleware();
app.SubscribeToEvents();

app.Run();