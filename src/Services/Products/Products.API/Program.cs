using System.Text.Json.Serialization;
using BuildingBlocks.Setup;
using BuildingBlocks.Swagger;
using Products.API.Endpoints;
using Products.Application;
using Products.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
    .AddAuthorization()
    .AddAuthentication(builder.Configuration)
    .AddSwagger(builder.Configuration, "Swagger", "Identity")
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Host.UseSerilog((_, configuration) => configuration.WriteTo.Console());


var app = builder.Build();

app.UseSwagger(app.Configuration, "Swagger");

app.UseAuthentication();
app.UseAuthorization();

var endpointRouteBuilder = app.MapApi();
endpointRouteBuilder.MapProductCategory();
endpointRouteBuilder.MapProduct();

app.UseFluentValidationMiddleware();
app.SubscribeToEvents();

app.Run();