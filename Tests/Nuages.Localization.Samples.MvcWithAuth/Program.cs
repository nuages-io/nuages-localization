#region

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Nuages.Localization.Config.Sources;

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
                if (hostingContext.HostingEnvironment.IsDevelopment())
                    config.AddJsonFile("appsettings.local.json", true);
                
                config.AddJsonFileTranslation("Locales/en-CA.json");
                config.AddJsonFileTranslation("Locales/fr-CA.json");


                //config.AddJsonHttpTranslation("https://s3.ca-central-1.amazonaws.com/public.nuages.org/fr.json");
            })
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}