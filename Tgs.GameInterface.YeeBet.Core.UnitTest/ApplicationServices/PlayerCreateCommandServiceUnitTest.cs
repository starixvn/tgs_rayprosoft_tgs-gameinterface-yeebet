using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

[TestClass]
public class PlayerCreateCommandServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new PlayerCreateCommand(
            ApiUrl: new Uri("http://api.98kbus.top/"),
            AppId: "xtdA15OTVGEP",
            SecretKey: "4885366DF30D880224FF6AA46FBDE4F0",
            PlayerCode: "4ucmqplxb029",
            ClientType: 1,
            Currency: "CNY",
            LanguageId: 1);

        var sut = new PlayerCreateCommandService(fakeGameApiClient);

        await sut
            .ExecuteAsync(command)
            .ConfigureAwait(false);

        await fakeGameApiClient
           .Received(1)
           .RegisterAsync(
               Arg.Is(command.ApiUrl),
               Arg.Is(command.AppId),
               Arg.Is(command.SecretKey),
               Arg.Is(command.ClientType),
               Arg.Is(command.Currency),
               Arg.Is(command.LanguageId),
               Arg.Is(command.PlayerCode))
           .ConfigureAwait(false);
    }
}
