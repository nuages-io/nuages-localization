using Microsoft.AspNetCore.Identity;

namespace Nuages.Localization.Samples.MvcWithAuth.Data;

public class ApplicationUser : IdentityUser
{
    public string Lang { get; set; } = "fr-CA";
}