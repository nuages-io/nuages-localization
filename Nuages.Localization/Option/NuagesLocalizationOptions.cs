#region

using System.Collections.Generic;

#endregion

namespace Nuages.Localization.Option;

public class NuagesLocalizationOptions
{
    // ReSharper disable once UnusedMember.Global
    public static string NuagesLocalization { get; set; } = "Nuages:Localization"; //Requires for options config
    public static string NuagesLocalizationValues { get; set; } = "Nuages:Localization:Values"; //Requires for options config
    
    public string FallbackCulture { get; set; } = "en-CA";
    
    public string? MissingTranslationUrl { get; set; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable once CollectionNeverUpdated.Global
    public List< string> Cultures { get; set; } = new();

    public string? AuthenticationScheme { get; set; }
    public string? LangClaim { get; set; }
    public string? LangCookie { get; set; } = ".nuageslang";
}