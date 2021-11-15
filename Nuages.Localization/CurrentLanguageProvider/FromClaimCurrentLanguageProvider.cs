#region

using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Options;

#endregion

namespace Nuages.Localization.CurrentLanguageProvider;

public class FromClaimCurrentLanguageProvider : ICurrentLanguageProvider
{
    private readonly NuagesLocalizationOptions _localizationOption;

    public FromClaimCurrentLanguageProvider(IOptions<NuagesLocalizationOptions> options)
    {
        _localizationOption = options.Value;
    }

    public string GetLanguage(ClaimsPrincipal principal)
    {
        var claimName = _localizationOption.LangClaim ?? "lang";
        var lang = _localizationOption.DefaultCulture;

        var langClaim = principal.Claims.SingleOrDefault(c => c.Type == claimName);
        if (langClaim != null) lang = langClaim.Value;

        return lang;
    }
}