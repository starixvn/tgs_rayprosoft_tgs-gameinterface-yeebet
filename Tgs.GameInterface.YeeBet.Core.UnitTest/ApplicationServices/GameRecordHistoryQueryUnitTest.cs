using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;
using Tgs.GameInterface.YeeBet.Core.Models;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

public class GameRecordHistoryQueryUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://api.98kbus.top/"),
            AppId: "xtdA15OTVGEP",
            SecretKey: "4885366DF30D880224FF6AA46FBDE4F0",
            Size: 1000);

        var sut = new GameRecordQueryService(fakeGameApiClient);

        _ = await sut
            .QueryAsync(query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        _ = await fakeGameApiClient
            .Received(1)
            .GetBetsAsync(
                 Arg.Is(query.ApiUrl),
                 Arg.Is(query.AppId),
                 Arg.Is(query.SecretKey),
                 Arg.Is(query.Size),
                 Arg.Any<CancellationToken>())
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<BetResult>();
    }
}
