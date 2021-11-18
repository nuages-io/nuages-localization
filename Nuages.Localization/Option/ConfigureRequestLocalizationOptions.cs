using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nuages.Localization.CultureProvider;

namespace Nuages.Localization.Option;

public class ConfigureRequestLocalizationOptions : IConfigureOptions<RequestLocalizationOptions>
{
    private readonly NuagesLocalizationOptions _nuagesLocalizationOptions;

    public ConfigureRequestLocalizationOptions(IOptions<NuagesLocalizationOptions> nuageLocalizationOptions)
    {
        _nuagesLocalizationOptions = nuageLocalizationOptions.Value;
    }

    public void Configure(RequestLocalizationOptions options)
    {
        var supportedUiCultures = _nuagesLocalizationOptions.Cultures.Select(c => new CultureInfo(c)).ToList();

        var defaultCulture = _nuagesLocalizationOptions.FallbackCulture;

        options.DefaultRequestCulture = new RequestCulture(defaultCulture, defaultCulture);

        options.SupportedCultures = supportedUiCultures;
        options.SupportedUICultures = supportedUiCultures;

            options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(context =>
            {
                var cultureProviders = context.RequestServices.GetServices<ICultureProvider>();

                string? culture = null;
                
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var provider in cultureProviders.Reverse())
                {
                    culture = provider.GetCulture();
                    if (!string.IsNullOrEmpty(culture))
                        break;
                }
               
                var result = new ProviderCultureResult(culture);
                
                return Task.FromResult(result)!;
            })
            
            );
    }
    

}