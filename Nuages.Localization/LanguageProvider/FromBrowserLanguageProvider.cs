using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace Nuages.Localization.LanguageProvider;

// ReSharper disable once UnusedType.Global
public class FromBrowserLanguageProvider : ILanguageProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IOptions<NuagesLocalizationOptions> _options;

    public FromBrowserLanguageProvider(IHttpContextAccessor contextAccessor, IOptions<NuagesLocalizationOptions> options)
    {
        _contextAccessor = contextAccessor;
        _options = options;
    }
    
    public string? GetLanguage()
    {
        var context = _contextAccessor.HttpContext!;
        string? lang = null;

        var cookieName = _options.Value.LangCookie;
        if (cookieName != null && context.Request.Cookies.ContainsKey(cookieName))
        {
            lang = context.Request.Cookies[cookieName];
        }
        else
        {
            lang = context.Request.Headers["Accept-Language"].ToString().Split(";")
                .FirstOrDefault()?.Split(",").FirstOrDefault();
        
        }

        var finalCulture = _options.Value.Cultures.FirstOrDefault(c => c.StartsWith($"{lang}-"));

        return string.IsNullOrEmpty(finalCulture) ?  "en-CA" : finalCulture;
    }
}