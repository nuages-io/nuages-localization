#region

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using Nuages.Localization.MissingLocalization;
using Nuages.Localization.Storage.Config.Providers;
using Xunit;

#endregion

namespace Nuages.Localization.Tests;

public class StringLocalizerFactoryTests
{
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly IStringLocalizerFactory _factory;

    private readonly Mock<IMissingLocalizationHandler> _missingLocalizationHandler =
        new();

    public StringLocalizerFactoryTests()
    {
        _factory = new StringLocalizerFactoryFromConfig(_configuration.Object, new [] {_missingLocalizationHandler.Object} );
    }

    [Fact]
    public void ShoutCreateStringLocalizerFactoryInstanceFromType()
    {
        var stringLocalizer = _factory.Create(typeof(StringLocalizerFactoryTests));
        Assert.NotNull(stringLocalizer);
    }

    [Fact]
    public void ShoutCreateStringLocalizerFactoryInstanceFromName()
    {
        var stringLocalizer = _factory.Create("baseNameIgnored", "locationIgnored");
        Assert.NotNull(stringLocalizer);
    }
}