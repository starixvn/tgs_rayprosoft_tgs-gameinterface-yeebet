using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;
using Tgs.GameInterface.YeeBet.Core.Models;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeLog = NullLogger<FundInGetService>.Instance;
        var fakeSnowflakeUtensil = Substitute.For<ISnowflakeUtensil>();
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundInQuery(
            ApiUrl: new Uri("http://api.98kbus.top/"),
            AppId: "xtdA15OTVGEP",
            SecretKey: "4885366DF30D880224FF6AA46FBDE4F0",
            PlayerCode: "4ucmqplxb029",
            Amount: 1m);

        var trsId = 1640526045735424000;
        _ = fakeSnowflakeUtensil
            .NextId()
            .Returns(trsId);

        var result = new DepositWithdrawResult(
            OrderNo: trsId.ToString(),
            Balance: 1,
            Response: "{}");

        _ = fakeGameApiClient
             .DepositWithdrawAsync(
                 Arg.Is(query.ApiUrl),
                 Arg.Is(query.AppId),
                 Arg.Is(query.SecretKey),
                 Arg.Is(query.PlayerCode),
                 Arg.Is(trsId),
                 Arg.Is(-query.Amount))
             .Returns(result);

        var sut = new FundInGetService(
            fakeLog,
            fakeSnowflakeUtensil,
            fakeGameApiClient);

        var actual = await sut
            .GetAsync(query)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(trsId.ToString(), actual.TransactionId);
    }
}
