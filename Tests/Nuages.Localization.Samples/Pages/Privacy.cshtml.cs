using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Nuages.Localization.Samples.Pages;

public class PrivacyModel : PageModel
{
    // ReSharper disable once NotAccessedField.Local
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    // ReSharper disable once UnusedMember.Global
    public void OnGet()
    {
    }
}