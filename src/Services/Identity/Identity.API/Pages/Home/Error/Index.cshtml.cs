using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Error;

[AllowAnonymous]
[SecurityHeaders]
public class Index(IIdentityServerInteractionService interaction, IWebHostEnvironment environment)
    : PageModel
{
    public ViewModel View { get; set; }

    public async Task OnGet(string errorId)
    {
        View = new ViewModel();

        // retrieve error details from identityserver
        var message = await interaction.GetErrorContextAsync(errorId);
        if (message != null)
        {
            View.Error = message;

            if (!environment.IsDevelopment())
            {
                // only show in development
                message.ErrorDescription = null;
            }
        }
    }
}