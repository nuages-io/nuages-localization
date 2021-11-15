#region

using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

#endregion

namespace Nuages.Localization.MissingLocalization;

// ReSharper disable once UnusedType.Global
public class MissingLocalizationHttpHandler : IMissingLocalizationHandler
{
    public const string CantPostMissingTranslation = "CantPostMissingTranslation";
    private readonly HttpClient? _client;
    private readonly NuagesLocalizationOptions _localizationOptions;

    public MissingLocalizationHttpHandler(IHttpClientFactory clientFactory, IOptions<NuagesLocalizationOptions> options)
    {
        _localizationOptions = options.Value;

        if (!string.IsNullOrEmpty(_localizationOptions.MissingTranslationUrl))
            _client = clientFactory.CreateClient();
    }

    public async Task Notify(string name)
    {
        if (_client != null)
        {
            var data = new
            {
                text = $"MissingTranslation:{name}"
            };

            var json = JsonSerializer.Serialize(data);

            var res = await _client.PostAsync(_localizationOptions.MissingTranslationUrl, new StringContent(json));

            if (res.StatusCode != HttpStatusCode.OK) throw new Exception(CantPostMissingTranslation);
        }
    }
}