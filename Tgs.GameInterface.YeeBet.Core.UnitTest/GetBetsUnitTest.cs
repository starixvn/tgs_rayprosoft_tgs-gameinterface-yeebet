using System.Net;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

public class GetBetsUnitTest
{
    [Fact]
    public async Task 查詢遊戲紀錄成功()
    {
        var apiUrl = new Uri("http://api.98kbus.top/");
        var appId = "xtdA15OTVGEP";
        var secretKey = "298511c51d785332d8d279998b79cd3c";
        var size = 1000;

        var fakeLog = NullLogger<GameApiClient>.Instance;

        var url = new Uri(apiUrl, $"/api/data/getbets");

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["appid"] = appId;
        query["size"] = size.ToString();

        var response = new
        {
            result = 0,
            desc = "succeed",
            total = 3,
            arraysize = 3,
            array = new[] {
                new {
                    gameid = 12,
                    createtime = 1679966728,
                    serialnumber = 889025,
                    userstatus = 0,
                    betpoint = "PLAYER",
                    betodds = 2.0,
                    userid = 329563,
                    commamount = 50.0,
                    gameroundid = 218943112,
                    uid = "xtNT48I",
                    settletime = 1679966757,
                    gameresult = "37;20;12;52;19;3",
                    winlost = 50.0,
                    gametype = 4,
                    currency = "MYR",
                    id = 889025,
                    state = 1,
                    describe = "329563:329507:312754:1",
                    gameno = 23032809251436,
                    bettype = 0,
                    cid = 329507,
                    username = "4ucmqplxb029",
                    betamount = 50.0
                },
                new {
                    gameid = 12,
                    createtime = 1679971289,
                    serialnumber = 889027,
                    userstatus = 0,
                    betpoint = "BANKER",
                    betodds = 1.0,
                    userid = 329563,
                    commamount = 0.0,
                    gameroundid = 218950739,
                    uid = "xtNT48I",
                    settletime = 1679971327,
                    gameresult = "25;4;39;1;0;19",
                    winlost = 0.0,
                    gametype = 4,
                    currency = "MYR",
                    id = 889027,
                    state = 1,
                    describe = "329563:329507:312754:1",
                    gameno = 23032810411844,
                    bettype = 0,
                    cid = 329507,
                    username = "4ucmqplxb029",
                    betamount = 60.0
                },
                new {
                    gameid = 12,
                    createtime = 1679971289,
                    serialnumber = 889026,
                    userstatus = 0,
                    betpoint = "PLAYER",
                    betodds = 1.0,
                    userid = 329563,
                    commamount = 0.0,
                    gameroundid = 218950739,
                    uid = "xtNT48I",
                    settletime = 1679971327,
                    gameresult = "25;4;39;1;0;19",
                    winlost = 0.0,
                    gametype = 4,
                    currency = "MYR",
                    id = 889026,
                    state = 1,
                    describe = "329563:329507:312754:1",
                    gameno = 23032810411844,
                    bettype = 0,
                    cid = 329507,
                    username = "4ucmqplxb029",
                    betamount = 60.0
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
            .GetBetsAsync(apiUrl, appId, secretKey, size)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }
}
