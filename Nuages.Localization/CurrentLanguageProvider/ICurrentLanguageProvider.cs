#region

using System.Security.Claims;

#endregion

namespace Nuages.Localization.CurrentLanguageProvider;

public interface ICurrentLanguageProvider
{
    string GetLanguage(ClaimsPrincipal principal);
}