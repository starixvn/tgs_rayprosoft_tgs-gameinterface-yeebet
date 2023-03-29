using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundStatusQuery(
            ApiUrl: new Uri("http://api.98kbus.top/"),
            AppId: "xtdA15OTVGEP",
            SecretKey: "4885366DF30D880224FF6AA46FBDE4F0",
            PlayerCode: "4ucmqplxb029",
            TrsId: "1640520550106664960");

        _ = fakeGameApiClient
             .DepositWithdrawHistoryAsync(
                 Arg.Is(query.ApiUrl),
                 Arg.Is(query.AppId),
                 Arg.Is(query.SecretKey),
                 Arg.Is(query.PlayerCode),
                 Arg.Is(query.TrsId.ToString()),
                 Arg.Any<CancellationToken>())
             .Returns(true);

        var sut = new FundStatusGetService(fakeGameApiClient);

        var actual = await sut
            .GetAsync(query)
            .ConfigureAwait(false);

        Assert.True(actual);
    }
}
