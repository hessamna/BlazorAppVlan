using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlazorApptToken.Datas;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(string? returnUrl = null)
    {
        await _signInManager.SignOutAsync();

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return Redirect("/login"); // مسیر صفحه لاگین
    }
}
