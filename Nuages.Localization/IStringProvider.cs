using Microsoft.Extensions.Localization;

namespace Nuages.Localization;

public interface IStringProvider
{
    IEnumerable<LocalizedString> GetAllStrings(string culture);
    string? GetString(string name, string culture);
}
