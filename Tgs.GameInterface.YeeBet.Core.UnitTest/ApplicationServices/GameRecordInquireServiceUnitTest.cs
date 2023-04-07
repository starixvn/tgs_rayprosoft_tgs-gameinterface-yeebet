using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;
using Tgs.GameInterface.YeeBet.Core.Models;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

public class GameRecordInquireServiceUnitTest
{
    [Fact]
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

        var sut = new GameRecordInquireService(fakeGameApiClient);

        _ = await sut
            .QueryAsync(query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordInfo>();

        _ = await fakeGameApiClient
            .Received(1)
            .BetsDetailAsync(
                 Arg.Is(query.ApiUrl),
                 Arg.Is(query.AppId),
                 Arg.Is(query.SecretKey),
                 Arg.Is(query.BeginTime),
                 Arg.Is(query.EndTime),
                 Arg.Is(query.PageSize),
                 Arg.Is(query.PageIndex))
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<BetDetailResult>();
    }
}
