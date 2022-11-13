#region

using System.Diagnostics.CodeAnalysis;
using Nuages.Localization.Option;
using Nuages.Localization.Storage.Config.Sources.FromSDK;

#endregion

namespace Nuages.Localization.Storage.Config.Sources;

[ExcludeFromCodeCoverage]
internal class HttpConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly HttpConfigurationSource _apiConfigurationSource;
    private readonly string _culture;
    private Timer? _timer;

    public HttpConfigurationProvider(HttpConfigurationSource apiConfigurationSource, string culture)
    {
        _apiConfigurationSource = apiConfigurationSource;
        _culture = culture;

        ConfigureTimer();
    }

    [ExcludeFromCodeCoverage]
    public void Dispose()
    {
        if (_timer == null)
            return;

        _timer.Change(Timeout.Infinite, 0);
        _timer.Dispose();
        Console.WriteLine("Dispose timer");
    }

    [ExcludeFromCodeCoverage]
    private void ConfigureTimer()
    {
        _timer = new Timer(_ => Load(),
            null,
            TimeSpan.FromSeconds(_apiConfigurationSource.ExpiresInSeconds),
            TimeSpan.FromSeconds(_apiConfigurationSource.ExpiresInSeconds));
    }

    public override void Load()
    {
        using HttpClient client = new();

        var stream = client.GetStreamAsync(_apiConfigurationSource.Url).Result;

        Data = JsonConfigurationFileParser.Parse(stream, $"{NuagesLocalizationOptions.NuagesLocalizationValues}:{_culture}")!;

        stream.Close();
    }
}