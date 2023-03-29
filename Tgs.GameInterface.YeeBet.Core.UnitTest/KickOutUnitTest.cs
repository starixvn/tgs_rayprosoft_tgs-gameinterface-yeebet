using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

public class KickOutUnitTest
{
    [Fact]
    public async Task 玩家登出成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var username = "4ucmqplxb029";
        var secretKey = "298511c51d785332d8d279998b79cd3c";

        var fakeLog = NullLogger<GameApiClient>.Instance;

        var url = new Uri(apiUrl, $"/api/kickout");

        var response = new
        {
            result = 0,
            desc = "succeed",
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, url.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient);

        await sut
            .KickOutAsync(apiUrl, appId, secretKey, username)
            .ConfigureAwait(false);
    }

    [Fact]
    public void 玩家登出失敗()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var username = "4ucmqplxb028";
        var secretKey = "298511c51d785332d8d279998b79cd3c";

        var fakeLog = NullLogger<GameApiClient>.Instance;

        var dict = new Dictionary<string, string>
        {
            { "appid",appId },
            { "username",username },
        };

        var sign = YeeBetHasher.ComputeHash(dict, secretKey);

        var url = new Uri(apiUrl, $"/api/kickout");

        var response = new
        {
            result = -1011,
            desc = "unregistered error!",
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, url.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient);

        _ = Assert.ThrowsAsync<Exception>(async () => await sut
            .KickOutAsync(apiUrl, appId, secretKey, username)
            .ConfigureAwait(false));
    }
}
