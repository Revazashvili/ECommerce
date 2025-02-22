using System.Text.Json.Serialization;
using Products.API.Endpoints;
using Products.Application;
using Products.Infrastructure;
using Services.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
    .AddAuthorization()
    .AddAuthentication(builder.Configuration);

builder.Services.AddSwagger(builder.Configuration);

builder.Host.UseSerilogLogging();
builder.Services.AddApplication(builder.Configuration)
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
endpointRouteBuilder.MapProductCategory();
endpointRouteBuilder.MapProduct();

app.UseFluentValidationMiddleware();
app.SubscribeToEvents();

app.Run();