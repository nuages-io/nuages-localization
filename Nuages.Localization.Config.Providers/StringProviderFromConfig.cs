using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Nuages.Localization.Config.Providers;

public class StringProviderFromConfig : IStringProvider
{
    private readonly IConfiguration _configuration;

    public StringProviderFromConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<LocalizedString> GetAllStrings(string language)
    {
        var lang = _configuration.GetSection(language);

        //The AsEnumerable extension method can't be mock with Moq. Covered by the integration tests
        return lang.AsEnumerable().Where(s => !string.IsNullOrEmpty(s.Value))
            .Select(s => new LocalizedString(s.Key, s.Value));
    }

    public string? GetString(string name, string language)
    {
        var normalizedName = name.Replace(".", ":");
        
        var value = _configuration.GetSection($"{language}:{normalizedName}");

        return value?.Value;
    }
}