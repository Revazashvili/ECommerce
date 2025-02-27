using System.Text.Json.Serialization;
using BuildingBlocks.Setup;
using BuildingBlocks.Swagger;
using Ordering.API.Endpoints;
using Ordering.Application;
using Ordering.Infrastructure;
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

app.UseSwagger(app.Configuration, "Swagger")
    .UseAuthentication()
    .UseAuthorization();

var endpointRouteBuilder = app.MapApi();
endpointRouteBuilder.MapOrder();

app.UseFluentValidationMiddleware();
app.UseApplication();

app.Run();