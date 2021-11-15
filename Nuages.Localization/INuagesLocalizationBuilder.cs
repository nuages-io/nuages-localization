using Microsoft.Extensions.DependencyInjection;

namespace Nuages.Localization;

public interface INuagesLocalizationBuilder
{
    IServiceCollection Services { get; }
}