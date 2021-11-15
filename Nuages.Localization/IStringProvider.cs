using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace Nuages.Localization;

public interface IStringProvider
{
    IEnumerable<LocalizedString> GetAllStrings(string language);
    string? GetString(string name, string language);
}
