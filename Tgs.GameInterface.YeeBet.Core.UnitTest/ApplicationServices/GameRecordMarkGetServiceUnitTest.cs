using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

public class GameRecordMarkGetServiceUnitTest
{
	private static readonly string[] _ExternalRoundIds = ["889025"];

	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new GameRecordMarkQuery(
			ApiUrl: new Uri("http://api.98kbus.top/"),
			AppId: "xtdA15OTVGEP",
			SecretKey: "4885366DF30D880224FF6AA46FBDE4F0",
			ExternalRoundIds: _ExternalRoundIds);

		var sut = new GameRecordMarkGetService(fakeGameApiClient);

		_ = await sut.GetAsync(query);

		_ = await fakeGameApiClient
			.Received(1)
			.RemoveBetsAsync(
				Arg.Is(query.ApiUrl),
				Arg.Is(query.AppId),
				Arg.Is(query.SecretKey),
				Arg.Is(query.ExternalRoundIds));
	}
}
