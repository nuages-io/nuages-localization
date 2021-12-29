#region

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Nuages.Localization.CultureProvider;
using Nuages.Localization.MissingLocalization;
using Nuages.Localization.Option;
using Nuages.Localization.Storage.Config.Providers;

#endregion

namespace Nuages.Localization;

[ExcludeFromCodeCoverage]
public static class LocalizationConfigExtensions
{
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public static ILocalizationBuilder AddStringProvider<T, TD>(this ILocalizationBuilder builder) where T : class, IStringLocalizerFactory where TD : class, IStringProvider
    {
        builder.Services.AddSingleton<IStringLocalizerFactory, T>();
        builder.Services.AddScoped<IStringProvider, TD>();

        return builder;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static ILocalizationBuilder AddDefaultStringProvider(this ILocalizationBuilder builder)
    {
        return AddStringProvider<StringLocalizerFactoryFromConfig, StringProviderFromConfig>(builder);
    }
    
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static ILocalizationBuilder AddNuagesLocalization(this IMvcBuilder mvcBuilder,
        IConfiguration configuration)
    {
        return AddNuagesLocalizationInternal(mvcBuilder, configuration, null);
    }

    // ReSharper disable once UnusedMember.Global
    public static ILocalizationBuilder AddNuagesLocalization(this IMvcBuilder mvcBuilder,
        IConfiguration configuration, Action<NuagesLocalizationOptions> configure)
    {
        return AddNuagesLocalizationInternal(mvcBuilder, configuration, configure);
    }
    
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static ILocalizationBuilder AddNuagesLocalization(this IMvcBuilder mvcBuilder,
       Action<NuagesLocalizationOptions> configure)
    {
        return AddNuagesLocalizationInternal(mvcBuilder, null, configure);
    }
    
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static ILocalizationBuilder AddNuagesLocalization(this IMvcBuilder mvcBuilder)
    {
        return AddNuagesLocalizationInternal(mvcBuilder, null, null);
    }
    
    private static ILocalizationBuilder AddNuagesLocalizationInternal(this IMvcBuilder mvcBuilder,
        IConfiguration? configuration, Action<NuagesLocalizationOptions>? configure)
    {
        var services = mvcBuilder.Services;
        
        mvcBuilder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(); 
        
        if (configuration != null)
        {
            services.Configure<NuagesLocalizationOptions>(
                configuration.GetSection(NuagesLocalizationOptions.NuagesLocalization));
        }
      
        if (configure != null)
            services.Configure(configure);
        
        services.AddSingleton<IConfigureOptions<RequestLocalizationOptions>, ConfigureRequestLocalizationOptions>();
        services.AddSingleton<IConfigureOptions<NuagesLocalizationOptions>, ConfigureLocalizationOptions>();
        
        services.PostConfigure<NuagesLocalizationOptions>(localizationOptions =>
        {
            var configErrors = ValidationErrors(localizationOptions).ToArray();
            // ReSharper disable once InvertIf
            if (configErrors.Any())
            {
                var aggregateErrors = string.Join(",", configErrors);
                var count = configErrors.Length;
                var configType = localizationOptions.GetType().Name;
                throw new ApplicationException(
                    $"Found {count} configuration error(s) in {configType}: {aggregateErrors}");
            }
        });

        AddHttpStuff(services);
        
        services.AddScoped<IStringLocalizer, StringLocalizer>();
        services.AddScoped(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
        services.AddSingleton<IMissingLocalizationHandler, MissingLocalizationConsoleHandler>();
        
        services.AddScoped<ICultureProvider, FromCulturesListCultureProvider>();
        services.AddScoped<ICultureProvider, FromFallbackCultureProvider>();
        services.AddScoped<ICultureProvider, FromBrowserCultureProvider>();
        services.AddScoped<ICultureProvider, FromAuthenticatedUserClaimCultureProvider>();

        var builder = new LocalizationBuilder(services);

        builder.AddDefaultStringProvider();

        return builder;
    }

    private static IEnumerable<string> ValidationErrors(object option)
    {
        var context = new ValidationContext(option, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(option, context, results, true);
        foreach (var validationResult in results) yield return validationResult.ErrorMessage ?? "?";
    }

    [ExcludeFromCodeCoverage]
    private static void AddHttpStuff(IServiceCollection services)
    {
        if (services.All(x => x.ServiceType != typeof(IHttpContextAccessor))) services.AddHttpContextAccessor();

        if (services.All(x => x.ServiceType != typeof(IHttpClientFactory))) services.AddHttpClient();
    }
}