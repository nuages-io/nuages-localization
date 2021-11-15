using System.Linq;
using Microsoft.Extensions.Options;
using Nuages.Localization.Option;

namespace Nuages.Localization.LanguageProvider;

// ReSharper disable once UnusedType.Global
public class FromCulturesListLanguageProvider : ILanguageProvider
{
    private readonly IOptions<NuagesLocalizationOptions> _localizationOptions;

    public FromCulturesListLanguageProvider(IOptions<NuagesLocalizationOptions> localizationOptions)
    {
        _localizationOptions = localizationOptions;
    }
    
    public string? GetLanguage()
    {
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (_localizationOptions.Value.Cultures.Any())
        {
            return _localizationOptions.Value.Cultures[0];
        }

        return null;
    }
}