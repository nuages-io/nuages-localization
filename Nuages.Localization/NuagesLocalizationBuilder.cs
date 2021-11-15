using Microsoft.Extensions.DependencyInjection;

namespace Nuages.Localization;

public class NuagesLocalizationBuilder : INuagesLocalizationBuilder
{
    public NuagesLocalizationBuilder(IServiceCollection services)
    {
        Services = services;
    }

    /// <inheritdoc />
    public IServiceCollection Services { get; }

}