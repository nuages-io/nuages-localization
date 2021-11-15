using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Nuages.Localization.Samples.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    // ReSharper disable once MemberCanBePrivate.Global
    public string? RequestId { get; set; }

    // ReSharper disable once UnusedMember.Global
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    // ReSharper disable once NotAccessedField.Local
    private readonly ILogger<ErrorModel> _logger;

    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    // ReSharper disable once UnusedMember.Global
    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}