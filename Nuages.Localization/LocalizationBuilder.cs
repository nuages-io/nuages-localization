namespace Nuages.Localization;

public class LocalizationBuilder : ILocalizationBuilder
{
    public LocalizationBuilder(IServiceCollection services)
    {
        Services = services;
    }

    /// <inheritdoc />
    public IServiceCollection Services { get; }

}