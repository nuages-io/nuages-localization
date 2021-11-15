#region

using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Nuages.Localization.Samples.MvcWithAuth.Models;

#endregion

namespace Nuages.Localization.Samples.MvcWithAuth.Controllers;

public class HomeController : Controller
{
    private readonly IStringLocalizer _stringLocalizer;

    public HomeController(IStringLocalizer stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
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
}