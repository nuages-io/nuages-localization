#region

using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Configuration;
using Nuages.Localization.Storage.Config.Sources.FromSDK;

#endregion

namespace Nuages.Localization.Storage.Config.Sources;

[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
public static class JsonLocalizationConfigurationExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    // ReSharper disable once UnusedMember.Global
    public static IConfigurationBuilder AddJsonFileTranslation(this IConfigurationBuilder builder, string path,
        string? culture = null, bool optional = false, bool reloadOnChange = false)
    {
        if (string.IsNullOrEmpty(culture)) 
            culture = Path.GetFileNameWithoutExtension(path);

        return builder.Add(new JsonConfigurationSource
        {
            Path = path,
            Culture = culture,
            Optional = optional,
            ReloadOnChange = reloadOnChange
        });
    }

    // ReSharper disable once UnusedMethodReturnValue.Global
    // ReSharper disable once UnusedMember.Global
    public static IConfigurationBuilder AddJsonHttpTranslation(
        this IConfigurationBuilder builder, string url, string? culture = null, int expireInSeconds = 3600)
    {
        if (string.IsNullOrEmpty(culture)) 
            culture = Path.GetFileNameWithoutExtension(url);

        return builder.Add(new HttpConfigurationSource
        {
            Url = url,
            Culture = culture,
            ExpiresInSeconds = expireInSeconds
        });
    }
}