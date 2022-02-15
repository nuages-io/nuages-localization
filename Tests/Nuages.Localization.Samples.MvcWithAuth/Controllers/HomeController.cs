#region

using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Nuages.Localization.Samples.MvcWithAuth.Data;
using Nuages.Localization.Samples.MvcWithAuth.Models;

#endregion

namespace Nuages.Localization.Samples.MvcWithAuth.Controllers;

public class HomeController : Controller
{
    private readonly IStringLocalizer _stringLocalizer;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(IStringLocalizer stringLocalizer, UserManager<ApplicationUser> userManager)
    {
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    [Authorize]
    public IActionResult GetAllStrings()
    {
        return Json(_stringLocalizer.GetAllStrings());
    }
    
    [Authorize]
    public async Task<ActionResult> ChangeLanguage(string lang)
    {
        var user = await _userManager.GetUserAsync(User);
        user.Lang = lang;
        await _userManager.UpdateAsync(user);

        return Redirect("~/");
    }
}