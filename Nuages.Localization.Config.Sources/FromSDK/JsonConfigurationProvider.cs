
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Nuages.Localization.Config.Sources.FromSDK;

public class JsonConfigurationProvider : FileConfigurationProvider
{
    private readonly string _language;

    /// <summary>
    ///     Initializes a new instance with the specified source.
    /// </summary>
    /// <param name="source">The source settings.</param>
    /// <param name="language">The language settings.</param>
    // ReSharper disable once SuggestBaseTypeForParameter
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public JsonConfigurationProvider(JsonConfigurationSource source, string language) : base(source)
    {
        _language = language;
    }

    /// <summary>
    ///     Loads the JSON data from a stream.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    public override void Load(Stream stream)
    {
        Data = JsonConfigurationFileParser.Parse(stream, $"{NuagesLocalizationOptions.NuagesLocalizationValues}:{_language}");
    }
}