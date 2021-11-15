#region

using Xunit;

#endregion

namespace Nuages.Localization.Tests;

public class FromClaimCurrentLanguageProviderTests
{
    //private readonly ILanguageProvider _languageProvider;

    // public FromClaimCurrentLanguageProviderTests()
    // {
    //     var someOptions = Options.Create(new NuagesLocalizationOptions
    //     {
    //         FallbackCulture = "en",
    //         LangClaim = "lang"
    //     });
    //     
    //     _languageProvider = new FromAuthenticatedUserClaimLanguageProvider(someOptions);
    // }

    [Fact]
    public void ShouldReturnCurrentLanguageWithSuccess()
    {
        // var claims = new List<Claim>
        // {
        //     new("lang", "fr")
        // };
        //
        // var identity = new ClaimsIdentity(claims, "TestAuthType");
        // var claimsPrincipal = new ClaimsPrincipal(identity);
        //
        // var res = _languageProvider.GetLanguage(claimsPrincipal);
        //
        // Assert.Equal("fr", res);
    }
}