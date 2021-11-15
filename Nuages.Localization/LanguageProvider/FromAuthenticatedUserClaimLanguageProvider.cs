#region

using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

#endregion

namespace Nuages.Localization.LanguageProvider;

// ReSharper disable once UnusedType.Global
public class FromAuthenticatedUserClaimLanguageProvider : ILanguageProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly NuagesLocalizationOptions _localizationOption;

    public FromAuthenticatedUserClaimLanguageProvider(IHttpContextAccessor contextAccessor, IOptions<NuagesLocalizationOptions> options)
    {
        _contextAccessor = contextAccessor;
        _localizationOption = options.Value;
    }

    public string? GetLanguage()
    {
        if (string.IsNullOrEmpty(_localizationOption.LangClaim) || string.IsNullOrEmpty(_localizationOption.AuthenticationScheme))
        {
            return null;
        }
        
        var authResult = _contextAccessor.HttpContext!.AuthenticateAsync(_localizationOption.AuthenticationScheme).Result;
        if (!authResult.Succeeded)
            return null;
        
        var claimName = _localizationOption.LangClaim!;
        string? lang = null;
        
        var langClaim = authResult.Principal.Claims.SingleOrDefault(c => c.Type == claimName);
        if (langClaim != null) 
            lang = langClaim.Value;
        
        return lang;

    }
}