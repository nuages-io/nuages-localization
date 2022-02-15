#region

#endregion

namespace Nuages.Localization.Option;

public class NuagesLocalizationOptions
{
    // ReSharper disable once UnusedMember.Global
    public static string NuagesLocalization { get; set; } = "Nuages:Localization"; //Requires for options config
    public static string NuagesLocalizationValues { get; set; } = "Nuages:Localization:Values"; //Requires for options config
    
    /// <summary>
    /// Used as the default culture when no ICultureProvider provide a value
    /// </summary>
    public string FallbackCulture { get; set; } = "en-CA";
    
    /// <summary>
    /// If provided, a POST request will be sent to this URL when a localized key is not found.
    /// Ex. if the key LastName is not found, a post will be sent to the url with the body { text : 'LastName' }
    /// </summary>
    public string? MissingTranslationUrl { get; set; }

    /// <summary>
    /// List of available cultures you are providing string value for.
    /// </summary>
    public List<string> Cultures { get; set; } = new();

    /// <summary>
    /// Optional: Used by FromAuthenticatedUserClaimCultureProvider. AuthenticationSchem authenticated agains to get principal claims.
    /// </summary>
    public string? LangClaimAuthenticationScheme { get; set; }
    
    /// <summary>
    /// Optional: Used by FromAuthenticatedUserClaimCultureProvider. Claim containing the current culture.
    /// </summary>
    public string? LangClaim { get; set; } = "NuagsLang";
    
    /// <summary>
    /// Optional: Used by FromBrowserCultureProvider.  The cookie that contain the current culture. 
    /// Usefull when a user return to your site and he is not logged in.
    /// If not provided, FromBrowserCultureProvider will look for the Accept-Languages header.
    /// </summary>
    public string? LangCookie { get; set; } = ".nuageslang";
}