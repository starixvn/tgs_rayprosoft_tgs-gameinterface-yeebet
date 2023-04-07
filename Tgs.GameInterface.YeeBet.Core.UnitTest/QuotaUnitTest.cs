using System.Net;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

[TestClass]
public class QuotaUnitTest
{
    [TestMethod]
    public async Task 查詢限紅成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var secretKey = "298511c51d785332d8d279998b79cd3c";

        var fakeLog = NullLogger<GameApiClient>.Instance;

        var url = new Uri(apiUrl, $"api/quota/list");

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["appid"] = appId;

        var response = new
        {
            result = 0,
            desc = "succeed",
            total = 5,
            arraysize = 5,
            array = new[] {
                new {
                    ID = 5,
                    up = 1000.0,
                    title = "10-1000",
                    down = 10.0
                },
                new {
                    ID = 6,
                    up = 2000.0,
                    title = "50-2000",
                    down = 50.0
                },
                new {
                    ID = 7,
                    up = 5000.0,
                    title = "100-5000",
                    down = 100.0
                },
                new {
                    ID = 8,
                    up = 10000.0,
                    title = "200-10000",
                    down = 200.0
                },
                new {
                    ID = 9,
                    up = 20000.0,
                    title = "500-20000",
                    down = 500.0
                }
            }
        };

        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();

        _ = handler.Expect(HttpMethod.Get, url.ToString())
            .WithQueryString(query.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var sut = new GameApiClient(fakeLog, fakeHttpClient);

        var actual = await sut
            .QuotaAsync(apiUrl, appId, secretKey)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any());
    }
}
