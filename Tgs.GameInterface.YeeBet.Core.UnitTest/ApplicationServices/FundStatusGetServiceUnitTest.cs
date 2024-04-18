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
			OrderId: 1640520550106664960);

		_ = fakeGameApiClient.DepositWithdrawHistoryAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			appId: Arg.Is(query.AppId),
			secretKey: Arg.Is(query.SecretKey),
			username: Arg.Is(query.PlayerCode),
			tradeno: Arg.Is(query.OrderId))
			.Returns(true);

		var sut = new FundStatusGetService(fakeGameApiClient);

		var actual = await sut.GetAsync(query);

		Assert.True(actual);
	}
}
