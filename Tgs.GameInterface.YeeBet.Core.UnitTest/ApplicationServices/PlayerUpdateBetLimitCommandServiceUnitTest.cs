using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

[TestClass]
public class PlayerUpdateBetLimitCommandServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new PlayerUpdateBetLimitCommand(
            ApiUrl: new Uri("http://api.98kbus.top/"),
            AppId: "xtdA15OTVGEP",
            SecretKey: "4885366DF30D880224FF6AA46FBDE4F0",
            PlayerCode: "4ucmqplxb029",
            Quotas: new int[] { 7, 8, 9 });

        var sut = new PlayerUpdateBetLimitCommandService(fakeGameApiClient);

        _ = await sut
            .GetAsync(command)
            .ConfigureAwait(false);

        await fakeGameApiClient
           .Received(1)
           .SetQuotaAsync(
               Arg.Is(command.ApiUrl),
               Arg.Is(command.AppId),
               Arg.Is(command.SecretKey),
               Arg.Is(command.PlayerCode),
               Arg.Is(command.Quotas))
           .ConfigureAwait(false);
    }
}
