using Microsoft.Extensions.Options;
using Nuages.Localization.Option;

namespace Nuages.Localization.CultureProvider;

// ReSharper disable once UnusedType.Global
public class FromFallbackCultureProvider : ICultureProvider
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static string FallbackCulture { get; set; } = "en-CA";
    
    private readonly IOptions<NuagesLocalizationOptions> _options;

    public FromFallbackCultureProvider(IOptions<NuagesLocalizationOptions> options)
    {
        _options = options;
    }
    
    // ReSharper disable once ReturnTypeCanBeNotNullable
    public string? GetCulture()
    {
        return !string.IsNullOrEmpty(_options.Value.FallbackCulture) ? _options.Value.FallbackCulture : FallbackCulture;
    }
}