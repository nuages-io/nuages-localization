#region

using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Nuages.Localization.Option;

#endregion

namespace Nuages.Localization.MissingLocalization;

// ReSharper disable once UnusedType.Global
public class MissingLocalizationHttpHandler : IMissingLocalizationHandler
{
    public const string CantPostMissingTranslation = "CantPostMissingTranslation";
    private readonly HttpClient? _client;
    private readonly NuagesLocalizationOptions _nuagesLocalizationOptions;

    public MissingLocalizationHttpHandler(IHttpClientFactory clientFactory, IOptions<NuagesLocalizationOptions> options)
    {
        _nuagesLocalizationOptions = options.Value;

        if (!string.IsNullOrEmpty(_nuagesLocalizationOptions.MissingTranslationUrl))
            _client = clientFactory.CreateClient();
    }

    public async Task Notify(string name)
    {
        if (_client != null)
        {
            var data = new
            {
                text = name
            };

            var json = JsonSerializer.Serialize(data);

            var res = await _client.PostAsync(_nuagesLocalizationOptions.MissingTranslationUrl, new StringContent(json));

            if (res.StatusCode != HttpStatusCode.OK) throw new Exception(CantPostMissingTranslation);
        }
    }
}