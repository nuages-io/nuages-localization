using Microsoft.Extensions.Options;

namespace Nuages.Localization.Option;

public class ConfigureLocalizationOptions : IConfigureOptions<NuagesLocalizationOptions>
{
    private readonly IConfiguration _configuration;

    public ConfigureLocalizationOptions(IConfiguration configuration)
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