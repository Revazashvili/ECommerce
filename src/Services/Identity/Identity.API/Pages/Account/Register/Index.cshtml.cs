using System.Security.Claims;
using Identity.API.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Register;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    [BindProperty] 
    public InputModel Input { get; set; }

    public Index(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet(string returnUrl)
    {
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if(Input.Button != "register")
            return Redirect("~/");
        
        var user = new ApplicationUser
        {
            UserName = Input.Username,
            PersonalNumber = Input.PersonalNumber,
            PhoneNumber = Input.Phone,
            Status = ApplicationUserStatus.Active,
            Email = $"{Input.Username}@gmail.com",
            EmailConfirmed = true
        };
        var result = _userManager.CreateAsync(user, Input.Password).Result;
        
        if (!result.Succeeded)
        {
            return RedirectToPage("/Home/Error/Index");
        }

        result = _userManager.AddClaimsAsync(user, new Claim[]
        {
            new(JwtClaimTypes.Name, Input.FullName),
            new Claim(JwtClaimTypes.WebSite, $"http://{Input.Username}.com"),
        }).Result;
        if (!result.Succeeded)
        {
            return RedirectToPage("/Home/Error/Index");
        }
        
        return Redirect("/Account/Login");
    }
}