using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nuages.Localization.CurrentLanguageProvider;

namespace Nuages.Localization;

public class ConfigureRequestLocalizationOptions : IConfigureOptions<RequestLocalizationOptions>
{
    private readonly NuagesLocalizationOptions _nuageLocalizationOptions;

    public ConfigureRequestLocalizationOptions(IOptions<NuagesLocalizationOptions> nuageLocalizationOptions)
    {
        _nuageLocalizationOptions = nuageLocalizationOptions.Value;
    }

    public void Configure(RequestLocalizationOptions options)
    {
        var supportedUiCultures = _nuageLocalizationOptions.Cultures.Select(c => new CultureInfo(c)).ToList();

        var defaultCulture = _nuageLocalizationOptions.DefaultCulture;

        options.DefaultRequestCulture = new RequestCulture(defaultCulture, defaultCulture);

        options.SupportedCultures = supportedUiCultures;
        options.SupportedUICultures = supportedUiCultures;

        if (_nuageLocalizationOptions.AddDefaultRequestCultureProvider)
            options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(context =>
            {
                var authResult = context.AuthenticateAsync().Result;
                
                if (!authResult.Succeeded /*context.User.Identity == null || !context.User.Identity.IsAuthenticated*/)
                {
                    if (context.Request.Cookies.ContainsKey(".lang"))
                    {
                        var browserLang = context.Request.Cookies[".lang"];

                        defaultCulture = NormalizeLanguage(browserLang);
                    }
                    else
                    {
                        var browserLang = context.Request.Headers["Accept-Language"].ToString().Split(";")
                            .FirstOrDefault()?.Split(",").FirstOrDefault();

                        defaultCulture = NormalizeLanguage(browserLang);
                    }

                    return Task.FromResult<ProviderCultureResult?>(
                        new ProviderCultureResult(defaultCulture)); //Return null, will go to the next provder
                }


                var languageProvider = context.RequestServices.GetService<ICurrentLanguageProvider>();

                var lang = languageProvider!.GetLanguage(authResult.Principal!);

                //Set the cookie, it will be used when visiting a page without being authenticated
                context.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1)
                    }
                );

                return Task.FromResult(new ProviderCultureResult(lang))!;
            }));
    }
    
    private static string NormalizeLanguage(string? browserLang)
    {
        return string.IsNullOrEmpty(browserLang) || browserLang.StartsWith("en") ? "en-CA" : "fr-CA";
    }
}