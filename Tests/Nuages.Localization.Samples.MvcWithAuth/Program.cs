#region

using System.Diagnostics.CodeAnalysis;
using Nuages.Localization.Storage.Config.Sources;

#endregion

namespace Nuages.Localization.Samples.MvcWithAuth;

[ExcludeFromCodeCoverage]
// ReSharper disable once ClassNeverInstantiated.Global
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.local.json", true);
                
                config.AddJsonFileTranslation("Locales/en-CA.json");
                config.AddJsonFileTranslation("Locales/fr-CA.json");

                // Fille may also be loaded using Http request
                //config.AddJsonHttpTranslation("https://s3.ca-central-1.amazonaws.com/public.nuages.org/fr.json");
            })
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}