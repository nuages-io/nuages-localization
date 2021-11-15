#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Nuages.Localization.LanguageProvider;
using Nuages.Localization.MissingLocalization;

#endregion

namespace Nuages.Localization;

[ExcludeFromCodeCoverage]
public static class LocalizationConfigExtensions
{
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static INuagesLocalizationBuilder AddStringProvider<T, TD>(this INuagesLocalizationBuilder builder) where T : class, IStringLocalizerFactory where TD : class, IStringProvider
    {
        builder.Services.AddSingleton<IStringLocalizerFactory, T>();
        builder.Services.AddScoped<IStringProvider, TD>();

        return builder;
    }
    
    // ReSharper disable once UnusedMember.Global
    public static INuagesLocalizationBuilder AddNuagesLocalization(this IServiceCollection services,
        IConfiguration configuration)
    {
        return AddNuagesLocalizationInternal(services, configuration, null);
    }

    // ReSharper disable once UnusedMember.Global
    public static INuagesLocalizationBuilder AddNuagesLocalization(this IServiceCollection services,
        IConfiguration configuration, Action<NuagesLocalizationOptions> configure)
    {
        return AddNuagesLocalizationInternal(services, configuration, configure);
    }
    
    // ReSharper disable once UnusedMember.Global
    public static INuagesLocalizationBuilder AddNuagesLocalization(this IServiceCollection services,
       Action<NuagesLocalizationOptions> configure)
    {
        return AddNuagesLocalizationInternal(services, null, configure);
    }
    
    private static INuagesLocalizationBuilder AddNuagesLocalizationInternal(this IServiceCollection services,
        IConfiguration? configuration, Action<NuagesLocalizationOptions>? configure)
    {
        if (configuration != null)
        {
            services.Configure<NuagesLocalizationOptions>(
                configuration.GetSection(NuagesLocalizationOptions.NuagesLocalization));
        }
      
        if (configure != null)
            services.Configure(configure);
        
        services.AddSingleton<IConfigureOptions<RequestLocalizationOptions>, ConfigureRequestLocalizationOptions>();
        services.AddSingleton<IConfigureOptions<NuagesLocalizationOptions>, ConfigureNuagesLocalizationOptions>();
        
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
        
        services.AddScoped<ILanguageProvider, FromCulturesListLanguageProvider>();
        services.AddScoped<ILanguageProvider, FromFallbackCultureLanguageProvider>();
        services.AddScoped<ILanguageProvider, FromBrowserLanguageProvider>();
        services.AddScoped<ILanguageProvider, FromAuthenticatedUserClaimLanguageProvider>();
        
        //services.AddScoped<ILanguageProvider, FromAuthenticatedUserClaimLanguageProvider>();

        return new NuagesLocalizationBuilder(services);
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