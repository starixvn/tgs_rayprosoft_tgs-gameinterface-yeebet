using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

[TestClass]
public class KickOutUnitTest
{
    [TestMethod]
    public async Task 玩家登出成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var username = "4ucmqplxb029";
        var secretKey = "298511c51d785332d8d279998b79cd3c";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHasher = Substitute.For<IYeeBetHasher>();

        var url = new Uri(apiUrl, $"/api/kickout");

        var dict = new Dictionary<string, string>
        {
            ["appid"] = appId,
            ["username"] = username,
        };

        var response = new
        {
            result = 0,
            desc = "succeed",
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, url.ToString())
            .With(req => req.Content?.Headers.ContentType?.MediaType == "application/x-www-form-urlencoded")
            .WithFormData(dict)
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient, fakeHasher);

        await sut
            .KickOutAsync(apiUrl, appId, secretKey, username)
            .ConfigureAwait(false);
    }

    [TestMethod]
    public void 玩家登出失敗()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var username = "4ucmqplxb028";
        var secretKey = "298511c51d785332d8d279998b79cd3c";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHasher = Substitute.For<IYeeBetHasher>();

        var url = new Uri(apiUrl, $"/api/kickout");

        var dict = new Dictionary<string, string>
        {
            ["appid"] = appId,
            ["username"] = username,
        };

        var response = new
        {
            result = -1011,
            desc = "unregistered error!",
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, url.ToString())
            .With(req => req.Content?.Headers.ContentType?.MediaType == "application/x-www-form-urlencoded")
            .WithFormData(dict)
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient, fakeHasher);

        _ = Assert.ThrowsExceptionAsync<Exception>(async () => await sut
            .KickOutAsync(apiUrl, appId, secretKey, username)
            .ConfigureAwait(false));
    }
}
