using Microsoft.AspNetCore.Identity;
using Nuages.Localization.CultureProvider;
using Nuages.Localization.Samples.MvcWithAuth.Data;

namespace Nuages.Localization.Samples.MvcWithAuth;

public class IdentityCultureProvider : ICultureProvider
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityCultureProvider(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string GetCulture()
    {
        var user = _httpContextAccessor.HttpContext!.User;

        var usr = _userManager.GetUserAsync(user).Result;

        return usr?.Lang;
    }
}