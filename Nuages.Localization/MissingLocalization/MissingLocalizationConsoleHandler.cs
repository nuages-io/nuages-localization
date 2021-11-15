#region

using System;
using System.Threading.Tasks;

#endregion

namespace Nuages.Localization.MissingLocalization;

public class MissingLocalizationConsoleHandler : IMissingLocalizationHandler
{
    public async Task Notify(string name)
    {
        await Task.Run(() =>
        {
            Console.WriteLine($"MissingTranslation:{name}");
        });

    }
}