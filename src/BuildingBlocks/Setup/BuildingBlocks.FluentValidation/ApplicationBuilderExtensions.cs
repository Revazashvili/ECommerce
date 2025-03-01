using Contracts.Mediatr.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.FluentValidation;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseFluentValidation(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next.Invoke();
            }
            catch (ValidationException exception)
            {
                var errorMessages = exception.Errors
                    .Where(failure => failure is not null)
                    .Select(failure => failure.ErrorMessage.Replace("'",""))
                    .ToList();

                var validationResult = new ValidationResult(errorMessages);
                await context.Response.WriteAsJsonAsync(validationResult);
            }
        });

        return app;
    }
}