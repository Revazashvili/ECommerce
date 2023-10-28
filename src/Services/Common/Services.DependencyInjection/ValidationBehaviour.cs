using Contracts.Mediatr.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Services.DependencyInjection;

public static class FluentValidationApplicationBuilderExtensions
{
    public static IApplicationBuilder UseFluentValidationMiddleware(this IApplicationBuilder app)
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