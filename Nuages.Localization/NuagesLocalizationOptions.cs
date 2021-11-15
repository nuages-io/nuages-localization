#region

using System.Collections.Generic;

#endregion

namespace Nuages.Localization;

public class NuagesLocalizationOptions
{
    // ReSharper disable once UnusedMember.Global
    public static string NuagesLocalization { get; set; } = "Nuages:Localization"; //Requires for options config
    public static string NuagesLocalizationValues { get; set; } = "Nuages:Localization:Values"; //Requires for options config
    
    public string DefaultCulture { get; set; } = "en";
    public string? LangClaim { get; set; }
    public string? MissingTranslationUrl { get; set; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable once CollectionNeverUpdated.Global
    public List< string> Cultures { get; set; } = new();

    public string? AuthenticationScheme { get; set; }
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public bool AddDefaultRequestCultureProvider { get; set; } = true;
}