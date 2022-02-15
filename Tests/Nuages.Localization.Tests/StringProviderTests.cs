#region

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Nuages.Localization.Option;
using Nuages.Localization.Storage.Config.Providers;
using Xunit;

#endregion

namespace Nuages.Localization.Tests;

public class StringProviderTests
{
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly StringProviderFromConfig _stringProviderFromConfig;

    public StringProviderTests()
    {
        _stringProviderFromConfig = new StringProviderFromConfig(_configuration.Object, Options.Create(new NuagesLocalizationOptions()));
    }

    [Fact]
    public void ShouldReturnStringWithSuccess()
    {
        const string lang = "en";
        const string name = "name";
        const string value = "value";

        _configuration.Setup(m => m.GetSection($"{NuagesLocalizationOptions.NuagesLocalizationValues}:{lang}:{name}").Value).Returns(value);

        var res = _stringProviderFromConfig.GetString(name, lang);
        Assert.Equal(value, res);
    }
}