using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

[TestClass]
public class DepositWithdrawUnitTest
{
    [TestMethod]
    public async Task 存款成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var secretKey = "298511c51d785332d8d279998b79cd3c";
        var username = "4ucmqplxb029";
        var tradeNo = 1640520550106664960;
        var amount = 1000;

        var fakeLog = NullLogger<GameApiClient>.Instance;

        var url = new Uri(apiUrl, $"/api/user/dw");

        var dict = new Dictionary<string, string>
        {
            ["appid"] = appId,
            ["username"] = username,
            ["amount"] = amount.ToString(),
            ["tradeno"] = tradeNo.ToString(),
        };

        var response = new
        {
            result = 0,
            desc = "succeed",
            balance = 11000.0,
            orderno = $"{tradeNo}",
            tradeno = $"{tradeNo}"
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, url.ToString())
            .With(req => req.Content?.Headers.ContentType?.MediaType == "application/x-www-form-urlencoded")
            .WithFormData(dict)
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient);

        var actual = await sut
            .DepositWithdrawAsync(apiUrl, appId, secretKey, username, tradeNo, amount)
            .ConfigureAwait(false);

        Assert.AreEqual(11000M, actual.Balance);
        Assert.AreEqual(tradeNo.ToString(), actual.OrderNo);
    }

    [TestMethod]
    public void 存款失敗_訂單號重複()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var secretKey = "298511c51d785332d8d279998b79cd3c";
        var username = "4ucmqplxkuyd";
        var tradeNo = 1640526045735424000;
        var amount = 10000;

        var fakeLog = NullLogger<GameApiClient>.Instance;

        var url = new Uri(apiUrl, $"/api/user/dw");

        var dict = new Dictionary<string, string>
        {
            ["appid"] = appId,
            ["username"] = username,
            ["amount"] = amount.ToString(),
            ["tradeno"] = tradeNo.ToString(),
        };

        var response = new
        {
            result = -1023,
            desc = "user orderno is exist!",
            balance = new decimal?(),
            orderno = $"{tradeNo}",
            tradeno = $"{tradeNo}"
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, url.ToString())
            .With(req => req.Content?.Headers.ContentType?.MediaType == "application/x-www-form-urlencoded")
            .WithFormData(dict)
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient);

        _ = Assert.ThrowsExceptionAsync<Exception>(async () => await sut
            .DepositWithdrawAsync(apiUrl, appId, secretKey, username, tradeNo, amount)
            .ConfigureAwait(false));
    }

    [TestMethod]
    public async Task 提款成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var secretKey = "298511c51d785332d8d279998b79cd3c";
        var username = "4ucmqplxb029";
        var tradeNo = 1640526045735424000;
        var amount = -100;

        var fakeLog = NullLogger<GameApiClient>.Instance;

        var url = new Uri(apiUrl, $"/api/user/dw");

        var dict = new Dictionary<string, string>
        {
            ["appid"] = appId,
            ["username"] = username,
            ["amount"] = amount.ToString(),
            ["tradeno"] = tradeNo.ToString(),
        };

        var response = new
        {
            result = 0,
            desc = "succeed",
            balance = 10950.0,
            orderno = $"{tradeNo}",
            tradeno = $"{tradeNo}"
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, url.ToString())
            .With(req => req.Content?.Headers.ContentType?.MediaType == "application/x-www-form-urlencoded")
            .WithFormData(dict)
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient);

        var actual = await sut
            .DepositWithdrawAsync(apiUrl, appId, secretKey, username, tradeNo, amount)
            .ConfigureAwait(false);

        Assert.AreEqual(10950.0M, actual.Balance);
        Assert.AreEqual(tradeNo.ToString(), actual.OrderNo);
    }

    [TestMethod]
    public void 提款失敗()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var secretKey = "298511c51d785332d8d279998b79cd3c";
        var username = "4ucmqplxb029";
        string? tradeNo = null;
        var amount = -10000;

        var fakeLog = NullLogger<GameApiClient>.Instance;

        var dict = new Dictionary<string, string>
        {
            ["appid"] = appId,
            ["username"] = username,
            ["amount"] = amount.ToString(),
        };

        var url = new Uri(apiUrl, $"/api/user/dw");

        var response = new
        {
            result = -1020,
            desc = "user dissatisfy balance!",
            balance = new decimal?(),
            orderno = tradeNo,
            tradeno = tradeNo
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Post, url.ToString())
            .With(req => req.Content?.Headers.ContentType?.MediaType == "application/x-www-form-urlencoded")
            .WithFormData(dict)
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient);

        _ = Assert.ThrowsExceptionAsync<Exception>(async () => await sut
            .DepositWithdrawAsync(apiUrl, appId, secretKey, username, long.MinValue, amount)
            .ConfigureAwait(false));
    }
}
