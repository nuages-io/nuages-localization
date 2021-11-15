#region

#endregion

using Microsoft.Extensions.Configuration;

namespace Nuages.Localization.Storage.Config.Sources.FromSDK;

public class JsonConfigurationSource : FileConfigurationSource
{
    public string Language { get; set; } = string.Empty;

    /// <summary>
    ///     Builds the <see cref="JsonConfigurationProvider" /> for this source.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder" />.</param>
    /// <returns>A <see cref="JsonConfigurationProvider" /></returns>
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);
        return new JsonConfigurationProvider(this, Language);
    }
}