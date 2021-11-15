#region

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Contrib.HttpClient;
using Nuages.Localization.MissingLocalization;
using Nuages.Localization.Option;
using Xunit;

#endregion

namespace Nuages.Localization.Tests;

public class MissingLocalizationHandlerTests
{
    private readonly IHttpClientFactory _factory;

    public MissingLocalizationHandlerTests()
    {
        var handler = new Mock<HttpMessageHandler>();

        handler.SetupRequest(HttpMethod.Post, "http://localhost")
            .ReturnsResponse(HttpStatusCode.OK);
        handler.SetupRequest(HttpMethod.Post, "http://localhost_invalid")
            .ReturnsResponse(HttpStatusCode.NotFound);

        _factory = handler.CreateClientFactory();
    }

    [Fact]
    public async Task ShouldNotifyWithSuccess()
    {
        var someOptions = Options.Create(new NuagesLocalizationOptions
        {
            MissingTranslationUrl = "http://localhost"
        });

        var handler = new MissingLocalizationHttpHandler(_factory, someOptions);
        await handler.Notify("not_found_ressource");
    }

    [Fact]
    public async Task ShouldNotifyWithFailure()
    {
        var someOptions = Options.Create(new NuagesLocalizationOptions
        {
            MissingTranslationUrl = "http://localhost_invalid"
        });

        var handler = new MissingLocalizationHttpHandler(_factory, someOptions);

        var res = await Assert.ThrowsAsync<Exception>(async () => { await handler.Notify("not_found_ressource"); });

        Assert.Equal(MissingLocalizationHttpHandler.CantPostMissingTranslation, res.Message);
    }
}