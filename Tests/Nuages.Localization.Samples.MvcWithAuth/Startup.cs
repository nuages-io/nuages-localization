#region

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nuages.Localization.Samples.MvcWithAuth.Data;

#endregion

namespace Nuages.Localization.Samples.MvcWithAuth;

[ExcludeFromCodeCoverage]
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(
                Configuration.GetConnectionString("DefaultConnection")));
        
        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        services.AddControllersWithViews();
        services.AddRazorPages()
            .AddNuagesLocalization(Configuration)
            .AddCultureProvider<IdentityCultureProvider>();

        // services.AddControllersWithViews()
        //     .AddNuagesLocalization(config =>
        //     {
        //         config.Cultures = new List<string> {"fr-CA","en-CA"}; //Available cultures, must have a least One (1)
        //         config.FallbackCulture = "fr-CA"; //Optional: Culture to use when no culture found using other mechanism. Otherwise use the first item in Cultures
        //         config.LangCookie = ".lang"; //Optional: Cookie to look for to determine the current culture (Use Accept-Language header otherwise)
        //         config.LangClaim = "lang"; //Optional: The claim to loof for in principal (from LangClaimAuthenticationScheme)
        //         config.LangClaimAuthenticationScheme = IdentityConstants.ApplicationScheme; //Optional: AuthenticationSchema for lang claim
        //         config.MissingTranslationUrl = "http://[my-webhook-url]"; //Optional: If provided, the Url will receve a notification when a localized string is not found. By default, that will be outputed to the Console.
        //     });     
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    // ReSharper disable once CA1822
    // ReSharper disable once CA1822
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        //ADD THIS LINE
        app.UseRequestLocalization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }
}