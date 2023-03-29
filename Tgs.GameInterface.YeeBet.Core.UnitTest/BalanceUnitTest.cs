using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

public class BalanceUnitTest
{
    [Fact]
    public async Task 查詢餘額成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var username = "4ucmqplxb029";
        var secretKey = "298511c51d785332d8d279998b79cd3c";

        var fakeLog = NullLogger<GameApiClient>.Instance;        

        var url = new Uri(apiUrl, $"/api/user/balance");

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
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient);

        var actual = await sut
            .BalanceAsync(apiUrl, appId, secretKey, username)
            .ConfigureAwait(false);

        Assert.Equal(10950M, actual.Balance);
        Assert.True(actual.IsPlayerExist);
    }

    [Fact]
    public async Task 玩家不存在()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var username = "4ucmqplxb028";
        var secretKey = "298511c51d785332d8d279998b79cd3c";

        var fakeLog = NullLogger<GameApiClient>.Instance;     

        var url = new Uri(apiUrl, $"/api/user/balance");

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
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient);

        var actual = await sut
            .BalanceAsync(apiUrl, appId, secretKey, username)
            .ConfigureAwait(false);

        Assert.Equal(0M, actual.Balance);
        Assert.False(actual.IsPlayerExist);
    }
}
