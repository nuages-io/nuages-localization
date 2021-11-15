#region

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Nuages.Localization.CurrentLanguageProvider;
using Xunit;

#endregion

namespace Nuages.Localization.Tests;

public class FromClaimCurrentLanguageProviderTests
{
    private readonly ICurrentLanguageProvider _currentLanguageProvider;

    public FromClaimCurrentLanguageProviderTests()
    {
        var someOptions = Options.Create(new NuagesLocalizationOptions
        {
            DefaultCulture = "en",
            LangClaim = "lang"
        });

        _currentLanguageProvider = new FromClaimCurrentLanguageProvider(someOptions);
    }

    [Fact]
    public void ShouldReturnCurrentLanguageWithSuccess()
    {
        var claims = new List<Claim>
        {
            new("lang", "fr")
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var res = _currentLanguageProvider.GetLanguage(claimsPrincipal);

        Assert.Equal("fr", res);
    }
}