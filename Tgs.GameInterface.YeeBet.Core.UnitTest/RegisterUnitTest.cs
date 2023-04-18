using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

[TestClass]
public class RegisterUnitTest
{
    [TestMethod]
    public async Task 創建玩家成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var secretKey = "298511c51d785332d8d279998b79cd3c";
        var clientType = 1;//pc
        var currency = "CNY";
        var languageId = 1;
        var username = "4ucmqplxkuyd";

        var fakeLog = NullLogger<GameApiClient>.Instance;

        var url = new Uri(apiUrl, $"/api/login");

        var dict = new Dictionary<string, string>
        {
            ["appid"] = appId,
            ["username"] = username,
            ["iscreate"] = 1.ToString(),
            ["currency"] = currency,
            ["clienttype"] = clientType.ToString(),
            ["language"] = languageId.ToString(),
        };

        var response = new
        {
            result = 0,
            desc = "succeed",
            openurl = "http://yibo.98kbus.top/direct/login/MTY3OTk3MDE1MF8zMDBfMzI5NTY4XzAwNzk1Qw==?d=5&l=1"
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, url.ToString())
            .With(req => req.Content?.Headers.ContentType?.MediaType == "application/x-www-form-urlencoded")
            .WithFormData(dict)
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient);

        await sut
            .RegisterAsync(apiUrl, appId, secretKey, clientType, currency, languageId, username)
            .ConfigureAwait(false);
    }

    [TestMethod]
    public async Task 用限額創建玩家成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var secretKey = "298511c51d785332d8d279998b79cd3c";
        var clientType = 1;//pc
        var currency = "CNY";
        var languageId = 1;
        var quotas = new int[] { 5, 6, 7 };
        var username = "4ucmqplxkuyd";

        var fakeLog = NullLogger<GameApiClient>.Instance;

        var url = new Uri(apiUrl, $"/api/login");

        var dict = new Dictionary<string, string>
        {
            ["appid"] = appId,
            ["username"] = username,
            ["iscreate"] = 1.ToString(),
            ["currency"] = currency,
            ["clienttype"] = clientType.ToString(),
            ["language"] = languageId.ToString(),
            ["quotas"] = string.Join(",", quotas)
        };

        var response = new
        {
            result = 0,
            desc = "succeed",
            openurl = "http://yibo.98kbus.top/direct/login/MTY3OTk3MDE1MF8zMDBfMzI5NTY4XzAwNzk1Qw==?d=5&l=1"
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, url.ToString())
            .With(req => req.Content?.Headers.ContentType?.MediaType == "application/x-www-form-urlencoded")
            .WithFormData(dict)
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient);

        await sut
            .RegisterAsync(apiUrl, appId, secretKey, clientType, currency, languageId, quotas, username)
            .ConfigureAwait(false);
    }
}
