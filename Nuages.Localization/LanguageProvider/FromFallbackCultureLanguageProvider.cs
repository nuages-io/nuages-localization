using Microsoft.Extensions.Options;

namespace Nuages.Localization.LanguageProvider;

// ReSharper disable once UnusedType.Global
public class FromFallbackCultureLanguageProvider : ILanguageProvider
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static string FallbackCulture { get; set; } = "en-CA";
    
    private readonly IOptions<NuagesLocalizationOptions> _options;

    public FromFallbackCultureLanguageProvider(IOptions<NuagesLocalizationOptions> options)
    {
        _options = options;
    }
    
    // ReSharper disable once ReturnTypeCanBeNotNullable
    public string? GetLanguage()
    {
        return !string.IsNullOrEmpty(_options.Value.FallbackCulture) ? _options.Value.FallbackCulture : FallbackCulture;
    }
}