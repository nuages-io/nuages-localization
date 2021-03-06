using Microsoft.Extensions.Options;
using Nuages.Localization.Option;

namespace Nuages.Localization.CultureProvider;

// ReSharper disable once UnusedType.Global
public class FromBrowserCultureProvider : ICultureProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IOptions<NuagesLocalizationOptions> _options;

    public FromBrowserCultureProvider(IHttpContextAccessor contextAccessor, IOptions<NuagesLocalizationOptions> options)
    {
        _contextAccessor = contextAccessor;
        _options = options;
    }
    
    // ReSharper disable once ReturnTypeCanBeNotNullable
    public string? GetCulture()
    {
        var context = _contextAccessor.HttpContext!;
        string? lang;

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

        string? finalCulture = null;

        if (!string.IsNullOrEmpty(lang))
        {
            //Try to find exact match
            if (lang.Contains("-"))
            {
                finalCulture = _options.Value.Cultures.FirstOrDefault(c => c == lang);
            }

            //Find first with closest match
            if (string.IsNullOrEmpty(finalCulture))
            {
                finalCulture = _options.Value.Cultures.FirstOrDefault(c => c.StartsWith($"{lang.Split('-').First()}-")); 
            }
        }

        var val = string.IsNullOrEmpty(finalCulture) ?  "en-CA" : finalCulture;

        return val;
    }
}