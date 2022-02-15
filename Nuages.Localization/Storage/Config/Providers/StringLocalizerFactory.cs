#region

using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Nuages.Localization.MissingLocalization;
using Nuages.Localization.Option;

#endregion

namespace Nuages.Localization.Storage.Config.Providers;

// ReSharper disable once UnusedType.Global
public class StringLocalizerFactoryFromConfig : IStringLocalizerFactory
{
    private readonly IConfiguration _configuration;
    private readonly IEnumerable<IMissingLocalizationHandler>  _missingLocalizationHandler;
    private readonly IOptions<NuagesLocalizationOptions> _options;

    public StringLocalizerFactoryFromConfig(IConfiguration configuration,
        IEnumerable<IMissingLocalizationHandler> missingLocalizationHandler,
        IOptions<NuagesLocalizationOptions> options)
    {
        _missingLocalizationHandler = missingLocalizationHandler;
        _configuration = configuration;
        _options = options;
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        return new StringLocalizer(new StringProviderFromConfig(_configuration, _options), _missingLocalizationHandler);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new StringLocalizer(new StringProviderFromConfig(_configuration, _options), _missingLocalizationHandler);
    }
}