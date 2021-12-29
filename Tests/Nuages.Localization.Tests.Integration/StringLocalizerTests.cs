#region

using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Nuages.Localization.Samples.MvcWithAuth;
using Xunit;

#endregion

namespace Nuages.Localization.Tests.Integration;

public class StringLocalizerTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _anonymousClient;
    private readonly HttpClient _client;

    public StringLocalizerTests(
        WebApplicationFactory<Startup> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", _ => { });
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        _anonymousClient = factory.WithWebHostBuilder(_ => { })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        _anonymousClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Test")]
    public async Task Get_AnonymousEndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Act
        var response = await _anonymousClient.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8",
            response.Content.Headers.ContentType!.ToString());
    }

    [Theory]
    [InlineData("/Home/Privacy")]
    [InlineData("/Home/Error")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8",
            response.Content.Headers.ContentType!.ToString());
    }

    [Theory]
    [InlineData("/Home/GetAllStrings")]
    public async Task Get_JsonEndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers!.ContentType!.ToString());
    }
}