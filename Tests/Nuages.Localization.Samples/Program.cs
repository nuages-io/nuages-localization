using Microsoft.AspNetCore.Mvc.Razor;
using Nuages.Localization;
using Nuages.Localization.Config.Providers;
using Nuages.Localization.Config.Sources;

var builder = WebApplication.CreateBuilder(args);

//Values loaded from AddJsonFileTranslation will overload value form appSettings.json
builder.Configuration.AddJsonFileTranslation("Locales/en-CA.json")
                     .AddJsonFileTranslation("Locales/fr-CA.json");

//This is standard stuff to enable localization
builder.Services.AddRazorPages()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(); 

//Wire up nuages-localization stuff using StringProviderFromConfig as the localization souurce
builder.Services.AddNuagesLocalization(options =>
    {
        options.Cultures = new List<string> { "en-CA", "fr-CA" };
        options.AuthenticationScheme = null;
        options.DefaultCulture = "en-CA";
        options.LangClaim = null;
        options.MissingTranslationUrl = null;
        options.AddDefaultRequestCultureProvider = true;

    }
).AddStringProvider<StringLocalizerFactoryFromConfig, StringProviderFromConfig>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRequestLocalization();

app.MapRazorPages();

app.Run();