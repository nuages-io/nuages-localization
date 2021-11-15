#region

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Nuages.Localization.MissingLocalization;

#endregion

namespace Nuages.Localization.Config.Providers;

// ReSharper disable once UnusedType.Global
public class StringLocalizerFactoryFromConfig : IStringLocalizerFactory
{
    private readonly IConfiguration _configuration;
    private readonly IEnumerable<IMissingLocalizationHandler>  _missingLocalizationHandler;

    public StringLocalizerFactoryFromConfig(IConfiguration configuration,
        IEnumerable<IMissingLocalizationHandler> missingLocalizationHandler)
    {
        _missingLocalizationHandler = missingLocalizationHandler;
        _configuration = configuration;
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        return new StringLocalizer(new StringProviderFromConfig(_configuration), _missingLocalizationHandler);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new StringLocalizer(new StringProviderFromConfig(_configuration), _missingLocalizationHandler);
    }
}