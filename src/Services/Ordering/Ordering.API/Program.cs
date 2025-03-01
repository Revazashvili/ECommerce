using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using BuildingBlocks.FluentValidation;
using BuildingBlocks.Swagger;
using Ordering.API.Endpoints;
using Ordering.Application;
using Ordering.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

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
app.UseApplication();

app.Run();