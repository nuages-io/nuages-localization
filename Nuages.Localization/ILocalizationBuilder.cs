using Microsoft.Extensions.DependencyInjection;

namespace Nuages.Localization;

public interface ILocalizationBuilder
{
    IServiceCollection Services { get; }
}