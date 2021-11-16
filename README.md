# Nuages.Localization

Nuages.Localization provide features to easily add localization to your ASP.net Core projects.

See [Globalization and localization in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-6.0) for more information on this subject.

By default, Nuages.Localization provide support for translations included in a generic JSON files. The internal storage used is the Configuration system. 
That means that translations can be loaded from appSettings.json files or from external source.


###Example appsettings.json

```json
{
  "Nuages" :
  {
    "Localization":
    {
      "Values" :
      {
        "fr-CA" :
        {
          "Item": "Il s'agit d'un test",
          "Parent":
          {
            "Child" : "C'est un item enfant"
          }
        },
        "en-CA" :
        {
          "Item": "This is a Test",
          "Parent":
          {
            "Child" : "This is a child item"
          }
        }
      }
    }
  }
}
```

# Installation


Add a reference to nuget package Nuages.Localization in your project.

```csharp
dotnet add package  Nuages.Localization
```

Modify _ViewStart.cshtml to add thos lines

```html

<!-- Add those lines-->
@using Microsoft.AspNetCore.Mvc.Localization
@inject IViewLocalizer Localizer

```

### .NET Core 3.1, 5.0

Modify your Startup.cs code (ConfigureServices) and add a call to AddNuagesLocalization() to your MvcBuilder.

````csharp
services.AddRazorPages()
    .AddNuagesLocalization(options =>
    {
    }
);
                
//or

services.AddControllersWithViews()
    .AddNuagesLocalization(options =>
    {
    }
);
````

Add a call to the Configure startup method

```csharp
app.UseRequestLocalization();
```


### .NET 6.0 (with merged Startup.cs and Programs.cs)

Modify your Program.cs code and add a call to AddNuagesLocalization() to your MvcBuilder.

```csharp
builder.Services.AddRazorPages()
                .AddNuagesLocalization();                
//or
builder.Services.AddControllersWithViews()
                .AddNuagesLocalization();        
```

Add a call to the Configure startup method

```csharp
app.UseRequestLocalization();
```

# Usage

### In .cshtml

```html
@Localizer["key-goes-here"]
```

### In controller or services loaded from DI

```csharp
public class HomeController : Controller
{
    private readonly IStringLocalizer _stringLocalizer;

    public HomeController(IStringLocalizer stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
    }

    public IActionResult Index()
    {
        Model.Value = _stringLocalizer["key-goes-here"];
        return View();
    }
}
```

### In data annotations

Just use annotations as usual. Values will be translated.

```csharp
public class RegisterViewModel
{
    [Required(ErrorMessage = "The Email field is required.")]
    [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The Password field is required.")]
    [StringLength(8, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}
```

# Load external JSON files

You may already have JSON files ready to use. You can load them directly from a JSON file included in your project of from an HTTP source

External files should provide **one language per file**. By default, the file name is used as the language. You may provide the language directly.

The format should be similar to this, starting at the root.

```json
{
  "Item": "Il s'agit d'un test",
  "Parent":
  {
    "Child" : "C'est un item enfant"
  }
}
```

### .NET Core 3.1, 5.0

You need to load those files in the CreateHostBuilder method of the Program class.

```csharp
public static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {           
            //To load from a file included in your project
            config.AddJsonFileTranslation("Locales/french-canadian.json","fr-CA");

            //To load from a file on the Web, use filename as language
            config.AddJsonHttpTranslation("https://here-goes-your-url.com/fr-CA.json");
        })
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}    
```

### .NET 6.0 (with merged Startup.cs and Programs.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

//Values loaded from AddJsonFileTranslation will overload value form appSettings.json
builder.Configuration.AddJsonFileTranslation("Locales/en-CA.json")
                     .AddJsonHttpTranslation("https://here-goes-your-url.com/fr-CA.json");      
```


# Custom loaders

By default, Nuages.Localization load the translation information from the Configuration system. That means you can implement a IConfigurationSource and ConfigurationProvider that 
load data from any source into the Configuration system and that will work just fine.

See the [Implement a custom configuration provider in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/custom-configuration-provider) samples on docs.microsoft.com

You need to put the values in a hierarchy that has the same root as **NuagesLocalizationOptions.NuagesLocalizationValues**

- See appsettings.json file sample at the top of this page. 
- See [HttpConfigurationProvider.cs](https://github.com/nuages-io/nuages-localization/blob/main/Nuages.Localization/Storage/Config/Sources/HttpConfigurationProvider.cs)
  and [HttpConfigurationSource.cs](https://github.com/nuages-io/nuages-localization/blob/main/Nuages.Localization/Storage/Config/Sources/HttpConfigurationSource.cs) for a sample.


# Custom reader

By default, Nuages.Localization read the translation information from the Configuration system. You may change that and use an alternative storage system.

You just have to provide your own IStringProvider and IStringLocalizerFactory implementations.

- See [StringLocalizerFactory.cs](https://github.com/nuages-io/nuages-localization/blob/main/Nuages.Localization/Storage/Config/Providers/StringLocalizerFactory.cs)
  and [StringProviderFromConfig.cs](https://github.com/nuages-io/nuages-localization/blob/main/Nuages.Localization/Storage/Config/Providers/StringProviderFromConfig.cs) for a sample.

# Setting the current culture

