using System.Net;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

[TestClass]
public class BalanceUnitTest
{
    [TestMethod]
    public async Task 查詢餘額成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var username = "4ucmqplxb029";
        var secretKey = "298511c51d785332d8d279998b79cd3c";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHasher = Substitute.For<IYeeBetHasher>();

        var url = new Uri(apiUrl, $"/api/user/balance");

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["appid"] = appId;
        query["username"] = username;

        var response = new
        {
            result = 0,
            desc = "ok",
            isonline = true,
            isplaying = false,
            balance = 10950.0
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Get, url.ToString())
            .WithQueryString(query.ToString()!)
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient, fakeHasher);

        var actual = await sut
            .BalanceAsync(apiUrl, appId, secretKey, username)
            .ConfigureAwait(false);

        Assert.AreEqual(10950M, actual.Balance);
        Assert.IsTrue(actual.IsPlayerExist);
    }

    [TestMethod]
    public async Task 玩家不存在()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var username = "4ucmqplxb028";
        var secretKey = "298511c51d785332d8d279998b79cd3c";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHasher = Substitute.For<IYeeBetHasher>();

        var url = new Uri(apiUrl, $"/api/user/balance");

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["appid"] = appId;
        query["username"] = username;

        var response = new
        {
            result = -1012,
            desc = "user not found!",
            isonline = new bool?(),
            isplaying = new bool?(),
            balance = new decimal?()
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Get, url.ToString())
            .WithQueryString(query.ToString()!)
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient, fakeHasher);

        var actual = await sut
            .BalanceAsync(apiUrl, appId, secretKey, username)
            .ConfigureAwait(false);

        Assert.AreEqual(0M, actual.Balance);
        Assert.IsFalse(actual.IsPlayerExist);
    }
}
