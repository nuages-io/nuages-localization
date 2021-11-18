using System.Linq;
using Microsoft.Extensions.Options;
using Nuages.Localization.Option;

namespace Nuages.Localization.CultureProvider;

// ReSharper disable once UnusedType.Global
public class FromCulturesListCultureProvider : ICultureProvider
{
    private readonly IOptions<NuagesLocalizationOptions> _localizationOptions;

    public FromCulturesListCultureProvider(IOptions<NuagesLocalizationOptions> localizationOptions)
    {
        _localizationOptions = localizationOptions;
    }
    
    public string? GetCulture()
    {
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (_localizationOptions.Value.Cultures.Any())
        {
            return _localizationOptions.Value.Cultures[0];
        }

        return null;
    }
}