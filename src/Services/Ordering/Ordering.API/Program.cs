using System.Text.Json.Serialization;
using Ordering.API.Endpoints;
using Ordering.Application;
using Ordering.Infrastructure;
using Services.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapApi();
app.MapOrder();

app.UseFluentValidationMiddleware();

app.Run();