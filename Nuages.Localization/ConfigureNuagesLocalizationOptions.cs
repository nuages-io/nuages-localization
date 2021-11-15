using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Nuages.Localization;

public class ConfigureNuagesLocalizationOptions : IConfigureOptions<NuagesLocalizationOptions>
{
    private readonly IConfiguration _configuration;

    public ConfigureNuagesLocalizationOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void Configure(NuagesLocalizationOptions options)
    {
        var section = _configuration.GetSection(NuagesLocalizationOptions.NuagesLocalizationValues);

        foreach (var s in section.GetChildren())
        {
            var key = s.Key;
            if (!options.Cultures.Contains(key))
                options.Cultures.Add(key);
        }
    }
}