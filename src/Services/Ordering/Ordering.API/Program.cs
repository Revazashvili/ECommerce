using System.Text.Json.Serialization;
using BuildingBlocks.Setup;
using Ordering.API.Endpoints;
using Ordering.Application;
using Ordering.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
    .AddAuthorization()
    .AddAuthentication(builder.Configuration);
builder.Services.AddSwagger(builder.Configuration);

builder.Host.UseSerilog((_, configuration) => configuration.WriteTo.Console());
builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.UseSwagger(app.Configuration);

app.UseAuthentication();
app.UseAuthorization();

var endpointRouteBuilder = app.MapApi();
endpointRouteBuilder.MapOrder();

app.UseFluentValidationMiddleware();
app.UseApplication();

app.Run();