using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nuages.Localization.LanguageProvider;

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

        var defaultCulture = _nuageLocalizationOptions.FallbackCulture;

        options.DefaultRequestCulture = new RequestCulture(defaultCulture, defaultCulture);

        options.SupportedCultures = supportedUiCultures;
        options.SupportedUICultures = supportedUiCultures;

            options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(context =>
            {
                var languageProviders = context.RequestServices.GetServices<ILanguageProvider>();

                string? lang = null;
                
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var provider in languageProviders.Reverse())
                {
                    lang = provider.GetLanguage();
                    if (!string.IsNullOrEmpty(lang))
                        break;
                }
               
                var result = new ProviderCultureResult(lang);
                
                return Task.FromResult(result)!;
            })
            
            );
    }
    

}