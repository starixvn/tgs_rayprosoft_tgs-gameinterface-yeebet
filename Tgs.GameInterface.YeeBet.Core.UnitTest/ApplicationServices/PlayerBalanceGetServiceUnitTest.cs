using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;
using Tgs.GameInterface.YeeBet.Core.Models;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var apiUrl = new Uri("http://api.98kbus.top/");
		var appId = "xtdA15OTVGEP";
		var secretKey = "4885366DF30D880224FF6AA46FBDE4F0";
		var username = "4ucmqplxb029";

		var query = new PlayerBalanceQuery(
			ApiUrl: apiUrl,
			AppId: appId,
			SecretKey: secretKey,
			Username: username);

		_ = fakeGameApiClient
			.BalanceAsync(apiUrl, appId, secretKey, username)
			.Returns(new BalanceResult(true, Balance: 11000.0M, false, false));

		var sut = new PlayerBalanceGetService(fakeGameApiClient);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayerExist);
		Assert.Equal(11000.0M, actual.Balance);
	}
}
