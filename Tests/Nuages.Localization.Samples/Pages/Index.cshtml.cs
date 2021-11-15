using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Nuages.Localization.Samples.Pages;

public class IndexModel : PageModel
{
    // ReSharper disable once NotAccessedField.Local
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    // ReSharper disable once UnusedMember.Global
    public void OnGet()
    {
    }
}