using System.Text.Json.Serialization;
using Products.API.Endpoints;
using Products.Application;
using Products.Infrastructure;
using Services.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapApi();
app.MapProductCategory();
app.MapProduct();

app.UseFluentValidationMiddleware();

app.Run();