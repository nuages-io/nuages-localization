#region

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Nuages.Localization.Option;

#endregion

namespace Nuages.Localization.CultureProvider;

// ReSharper disable once UnusedType.Global
public class FromAuthenticatedUserClaimCultureProvider : ICultureProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly NuagesLocalizationOptions _nuagesLocalizationOption;

    public FromAuthenticatedUserClaimCultureProvider(IHttpContextAccessor contextAccessor, IOptions<NuagesLocalizationOptions> options)
    {
        _contextAccessor = contextAccessor;
        _nuagesLocalizationOption = options.Value;
    }

    public string? GetCulture()
    {
        if (string.IsNullOrEmpty(_nuagesLocalizationOption.LangClaim) || string.IsNullOrEmpty(_nuagesLocalizationOption.AuthenticationScheme))
        {
            return null;
        }
        
        var authResult = _contextAccessor.HttpContext!.AuthenticateAsync(_nuagesLocalizationOption.AuthenticationScheme).Result;
        if (!authResult.Succeeded)
            return null;
        
        var claimName = _nuagesLocalizationOption.LangClaim!;
        string? lang = null;
        
        var langClaim = authResult.Principal.Claims.SingleOrDefault(c => c.Type == claimName);
        if (langClaim != null) 
            lang = langClaim.Value;
        
        return lang;

    }
}