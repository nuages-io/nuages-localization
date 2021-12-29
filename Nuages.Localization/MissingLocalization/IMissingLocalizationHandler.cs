#region

#endregion

namespace Nuages.Localization.MissingLocalization;

public interface IMissingLocalizationHandler
{
    Task Notify(string name);
}