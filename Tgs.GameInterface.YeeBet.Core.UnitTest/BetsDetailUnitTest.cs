using System.Net;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

public class BetsDetailUnitTest
{
    [Fact]
    public async Task 查詢遊戲紀錄成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var secretKey = "298511c51d785332d8d279998b79cd3c";
        var beginTime = new DateTimeOffset(2023, 04, 07, 11, 0, 24, TimeSpan.FromHours(+8));
        var endTime = new DateTimeOffset(2023, 04, 07, 11, 0, 25, TimeSpan.FromHours(+8));
        var pageSize = 1000;
        var pageIndex = 0;

        var fakeLog = NullLogger<GameApiClient>.Instance;

        var url = new Uri(apiUrl, $"/api/record/bets/detail");

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["appid"] = appId;
        query["begintime"] = beginTime.ToUnixTimeSeconds().ToString();
        query["endtime"] = endTime.ToUnixTimeSeconds().ToString();
        query["size"] = pageSize.ToString();
        query["index"] = pageIndex.ToString();

        var response = new
        {
            result = 0,
            desc = "ok",
            total = 1,
            arraysize = 1,
            array = new[] {
                new {
                    gameid = 2940,
                    createtime = "2023-04-07 11:00:24",
                    betpoint = "PLAYER",
                    betodds = 2.0M,
                    userid = 329563,
                    commamount = 50.0M,
                    gameroundid = 221212503,
                    uid = "xtNT48I",
                    gameresult = "14;29;27;12;4;24",
                    winlost = -50.0M,
                    balance = 11360.0M,
                    gametype = 4,
                    nickname = "径芜矢温砌肤逊铭",
                    gamestate = 5,
                    id = 892945,
                    state = 1,
                    updatetime = "2023-04-07 11:00:24",
                    bettype = 0,
                    gameno = "23040711001706",
                    betamount = 50.0M,
                    username = "4ucmqplxb029",
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
            .BetsDetailAsync(apiUrl, appId, secretKey, beginTime, endTime, pageSize, pageIndex)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }
}
