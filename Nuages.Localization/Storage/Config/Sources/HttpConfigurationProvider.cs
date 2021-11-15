#region

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Nuages.Localization.Storage.Config.Sources.FromSDK;

#endregion

namespace Nuages.Localization.Storage.Config.Sources;

[ExcludeFromCodeCoverage]
internal class HttpConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly HttpConfigurationSource _apiConfigurationSource;
    private readonly string _language;
    private Timer? _timer;

    public HttpConfigurationProvider(HttpConfigurationSource apiConfigurationSource, string language)
    {
        _apiConfigurationSource = apiConfigurationSource;
        _language = language;

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

        Data = JsonConfigurationFileParser.Parse(stream, _language);

        stream.Close();
    }
}