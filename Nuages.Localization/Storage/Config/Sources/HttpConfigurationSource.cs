#region

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

#endregion

namespace Nuages.Localization.Storage.Config.Sources;

[ExcludeFromCodeCoverage]
public class HttpConfigurationSource : IConfigurationSource
{
    public string Culture { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int ExpiresInSeconds { get; set; }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new HttpConfigurationProvider(this, Culture);
    }
}