using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

public class PlayerCreateByBetLimitCommandServiceUnitTest
{
	private static readonly int[] _Quotas = [5, 6, 7];

	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var command = new PlayerCreateByBetLimitCommand(
			ApiUrl: new Uri("http://api.98kbus.top/"),
			AppId: "xtdA15OTVGEP",
			SecretKey: "4885366DF30D880224FF6AA46FBDE4F0",
			PlayerCode: "4ucmqplxb029",
			ClientType: 1,
			Currency: "CNY",
			LanguageId: 1,
			Quotas: _Quotas);

		var sut = new PlayerCreateByBetLimitCommandService(fakeGameApiClient);

		await sut.ExecuteAsync(command);

		await fakeGameApiClient
		   .Received(1)
		   .RegisterAsync(
			   Arg.Is(command.ApiUrl),
			   Arg.Is(command.AppId),
			   Arg.Is(command.SecretKey),
			   Arg.Is(command.ClientType),
			   Arg.Is(command.Currency),
			   Arg.Is(command.LanguageId),
			   Arg.Is(command.Quotas),
			   Arg.Is(command.PlayerCode));
	}
}
