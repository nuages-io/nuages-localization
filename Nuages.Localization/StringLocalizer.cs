#region

using System.Globalization;
using Microsoft.Extensions.Localization;
using Nuages.Localization.MissingLocalization;

#endregion

namespace Nuages.Localization;

public class StringLocalizer : IStringLocalizer
{
    private readonly IEnumerable<IMissingLocalizationHandler> _missingLocalizationHandler;
    private readonly IStringProvider _stringProvider;

    public StringLocalizer(IStringProvider stringProvider, IEnumerable<IMissingLocalizationHandler> missingLocalizationHandler)
    {
        _stringProvider = stringProvider;
        _missingLocalizationHandler = missingLocalizationHandler;
    }

    public LocalizedString this[string name] => new(name, GetString(name), false);

    public LocalizedString this[string name, params object[] arguments] =>
        new(name, string.Format(GetString(name), arguments), false);

    public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
    {
        return _stringProvider.GetAllStrings(CultureInfo.CurrentCulture.Name);
    }

    public IStringLocalizer WithCulture(CultureInfo culture)
    {
        CultureInfo.DefaultThreadCurrentCulture = culture;
        return new StringLocalizer(_stringProvider, _missingLocalizationHandler);
    }

    private string GetString(string name)
    {
        var culture = CultureInfo.CurrentCulture.Name;

        var value = _stringProvider.GetString(name, culture);

        if (string.IsNullOrEmpty(value))
        {
            var parts = culture.Split('-');
            if (parts[0] != culture) 
                value = _stringProvider.GetString(name, parts[0]);
        }

        // ReSharper disable once InvertIf
        if (string.IsNullOrEmpty(value))
        {
            value = name;

            foreach (var missingLocalizationHandler in _missingLocalizationHandler)
            {
                missingLocalizationHandler.Notify($"{culture}:{name}");
            }
        }

        return value;
    }
}