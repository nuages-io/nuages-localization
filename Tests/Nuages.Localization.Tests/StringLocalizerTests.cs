#region

using System.Globalization;
using Microsoft.Extensions.Localization;
using Moq;
using Nuages.Localization.MissingLocalization;
using Xunit;

#endregion

namespace Nuages.Localization.Tests;

public class StringLocalizerTests
{
    private readonly List<LocalizedString> _allStrings = new()
    {
        new LocalizedString("name", "value")
    };

    private readonly Mock<IMissingLocalizationHandler>
        _missingTranslation = new();

    private readonly StringLocalizer _stringLocalizer;

    private readonly Mock<IStringProvider> _stringProvider = new();

    public StringLocalizerTests()
    {
        _stringLocalizer = new StringLocalizer(_stringProvider.Object, new [] {_missingTranslation.Object} );
        CultureInfo.CurrentCulture = new CultureInfo("en-CA");
    }

    [Fact]
    public void ShouldReturnAllStringsWithSuccess()
    {
        _stringProvider.Setup(s => s.GetAllStrings(CultureInfo.CurrentCulture.Name)).Returns(_allStrings);

        var res = _stringLocalizer.GetAllStrings().ToList();

        Assert.Single(res);
        Assert.Equal(_allStrings.First().Name, res.First().Name);
    }

    [Fact]
    public void ShouldReturnStringValueWithSuccess()
    {
        const string name = "name";

        _stringProvider.Setup(s => s.GetString(name, CultureInfo.CurrentCulture.Name)).Returns("value");

        var res = _stringLocalizer.GetString(name);

        Assert.Equal(_allStrings.First().Name, res.Name);
    }

    [Fact]
    public void ShouldReturnStringValueFromBaseCultureWithSuccess()
    {
        const string name = "name";

        _stringProvider.Setup(s => s.GetString(name, "en")).Returns("value");

        var res = _stringLocalizer.GetString(name);

        Assert.Equal(_allStrings.First().Name, res.Name);
    }

    [Fact]
    public void ShouldReturnNameStringNotFound()
    {
        const string name = "not_found_name";

        _stringProvider.Setup(s => s.GetString(name, CultureInfo.CurrentCulture.Name)).Returns("");

        var res = _stringLocalizer.GetString(name);

        Assert.Equal(name, res.Name);
    }

    [Fact]
    public void ShouldReturnStringValueWithParamWithSuccess()
    {
        const string name = "name_with_param";

        _stringProvider.Setup(s => s.GetString(name, CultureInfo.CurrentCulture.Name))
            .Returns("value {0}");

        var res = _stringLocalizer[name, "abc"];

        Assert.Equal("value abc", res);
    }

    [Fact]
    public void ShouldReturnNewLocalizer()
    {
        var localizer = _stringLocalizer.WithCulture(new CultureInfo("fr"));
        Assert.NotNull(localizer);
    }
}