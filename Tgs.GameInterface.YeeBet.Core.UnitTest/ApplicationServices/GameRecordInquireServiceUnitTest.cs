using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;
using Tgs.GameInterface.YeeBet.Core.Models;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

[TestClass]
public class GameRecordInquireServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordInquire(
            ApiUrl: new Uri("http://api.98kbus.top/"),
            AppId: "xtdA15OTVGEP",
            SecretKey: "4885366DF30D880224FF6AA46FBDE4F0",
            BeginTime: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndTime: DateTimeOffset.UtcNow,
            PageSize: 1000,
            PageIndex: 0);

        var result = new BetsDetailResult
        {
            Total = 2,
            ArraySize = 1,
            Data = new BetDetailResult[]
            {
                new(
                    ExternalRoundId: "893018",
                    ExternalGameId: "3006",
                    PlayerCode: "4ucmqplxb029",
                    ExternalCreatedOn: new DateTimeOffset(2023,04,07,15,15,28,TimeSpan.FromHours(+8)),
                    ExternalClosedOn:  new DateTimeOffset(2023,04,07,15,15,28,TimeSpan.FromHours(+8)),
                    Bet: 50,
                    ValidBet: 50,
                    TurnOver: 50,
                    WinLoss: -50,
                    AdditionalData: "",
                    IsVoid: false,
                    IsClosed: true)
            }
        };

        _ = fakeGameApiClient
            .BetsDetailAsync(query.ApiUrl, query.AppId, query.SecretKey, query.BeginTime, query.EndTime, query.PageSize, query.PageIndex)
                    .Returns(result);

        var sut = new GameRecordInquireService(fakeGameApiClient);

        var actual = await sut
            .GetAsync(query)
                    .ConfigureAwait(false);

        Assert.AreEqual(2, actual.Total);
        Assert.AreEqual(1, actual.ArraySize);
        Assert.IsTrue(actual.Data.Any());
    }
}
