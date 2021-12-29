using Microsoft.Extensions.Localization;
using Nuages.Localization.Option;

namespace Nuages.Localization.Storage.Config.Providers;

public class StringProviderFromConfig : IStringProvider
{
    private readonly IConfiguration _configuration;

    public StringProviderFromConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<LocalizedString> GetAllStrings(string culture)
    {
        var lang = _configuration.GetSection(culture);

        //The AsEnumerable extension method can't be mock with Moq. Covered by the integration tests
        return lang.AsEnumerable().Where(s => !string.IsNullOrEmpty(s.Value))
            .Select(s => new LocalizedString(s.Key, s.Value));
    }

    public string? GetString(string name, string culture)
    {
        var normalizedName = name.Replace(".", ":");
        
        var value = _configuration.GetSection($"{NuagesLocalizationOptions.NuagesLocalizationValues}:{culture}:{normalizedName}");

        return value?.Value;
    }
}