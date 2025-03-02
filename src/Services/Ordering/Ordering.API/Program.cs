using System.Text.Json.Serialization;
using BuildingBlocks.FluentValidation;
using BuildingBlocks.Swagger;
using Microsoft.IdentityModel.JsonWebTokens;
using Ordering.API.Endpoints;
using Ordering.Application;
using Ordering.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// prevent JWT claim keys getting mapped to the XML soap scheme URLs
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

var identitySection = builder.Configuration.GetSection("Identity");


builder.Services.AddEndpointsApiExplorer()
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication().AddJwtBearer(options =>
    {

        options.Authority = identitySection["Url"];
        options.RequireHttpsMetadata = false;
        options.Audience = identitySection["Audience"];
        options.TokenValidationParameters.ValidateAudience = false;
    });
    
builder.Services.AddSwagger(builder.Configuration, "Swagger", "Identity")
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

app.UseFluentValidation();

app.Run();