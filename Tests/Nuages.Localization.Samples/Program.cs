using Nuages.Localization;
using Nuages.Localization.Storage.Config.Sources;

var builder = WebApplication.CreateBuilder(args);

//This is standard stuff to enable localization
builder.Services.AddRazorPages()
                .AddNuagesLocalization();

// builder.Services..AddControllersWithViews()
//     .AddNuagesLocalization(config =>
//     {
//         config.Cultures = new List<string> {"fr-CA","en-CA"}; //Available cultures, must have a least One (1)
//         config.FallbackCulture = "fr-CA"; //Optional: Culture to use when no culture found using other mechanism. Otherwise use the first item in Cultures
//         config.LangCookie = ".lang"; //Optional: Cookie to look for to determine the current culture (Use Accept-Language header otherwise)
//         config.MissingTranslationUrl = "http://[my-webhook-url]"; //Optional: If provided, the Url will receve a notification when a localized string is not found. By default, that will be outputed to the Console.
//     });   

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