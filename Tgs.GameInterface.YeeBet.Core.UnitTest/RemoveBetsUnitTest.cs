using System.Net;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

[TestClass]
public class RemoveBetsUnitTest
{
    [TestMethod]
    public async Task 移除遊戲紀錄成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var secretKey = "298511c51d785332d8d279998b79cd3c";
        var ids = new[] { "889025" };

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHasher = Substitute.For<IYeeBetHasher>();

        var url = new Uri(apiUrl, $"/api/data/removebets");

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["appid"] = appId;
        query["ids"] = "889025";

        var response = new
        {
            result = 0,
            desc = "ok"
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Get, url.ToString())
            .WithQueryString(query.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient, fakeHasher);

        var actual = await sut
            .RemoveBetsAsync(apiUrl, appId, secretKey, ids)
            .ConfigureAwait(false);

        Assert.IsTrue(actual);
    }
}
