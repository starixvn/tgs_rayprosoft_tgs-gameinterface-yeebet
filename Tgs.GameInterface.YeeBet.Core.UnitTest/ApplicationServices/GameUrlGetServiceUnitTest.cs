using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

[TestClass]
public class GameUrlGetServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameUrlQuery(
            ApiUrl: new Uri("http://api.98kbus.top/"),
            AppId: "xtdA15OTVGEP",
            SecretKey: "4885366DF30D880224FF6AA46FBDE4F0",
            ClientType: 1,
            LanguageId: 1,
            PlayerCode: "4ucmqplxb029");

        var sut = new GameUrlGetService(fakeGameApiClient);

        _ = await sut
            .GetAsync(query)
            .ConfigureAwait(false);

        _ = await fakeGameApiClient
            .Received(1)
            .LoginAsync(
                Arg.Is(query.ApiUrl),
                Arg.Is(query.AppId),
                Arg.Is(query.SecretKey),
                Arg.Is(query.ClientType),
                Arg.Is(query.LanguageId),
                Arg.Is(query.PlayerCode))
            .ConfigureAwait(false);
    }
}
