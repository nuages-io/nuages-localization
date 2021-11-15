using Nuages.Localization;
using Nuages.Localization.Storage.Config.Sources;

var builder = WebApplication.CreateBuilder(args);

//Values loaded from AddJsonFileTranslation will overload value form appSettings.json
builder.Configuration.AddJsonFileTranslation("Locales/en-CA.json")
                     .AddJsonFileTranslation("Locales/fr-CA.json");

//This is standard stuff to enable localization
builder.Services.AddRazorPages()
                .AddNuagesLocalization();

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