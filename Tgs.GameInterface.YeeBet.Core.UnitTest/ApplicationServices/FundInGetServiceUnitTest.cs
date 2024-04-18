using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;
using Tgs.GameInterface.YeeBet.Core.Models;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new FundInQuery(
			ApiUrl: new Uri("http://api.98kbus.top/"),
			AppId: "xtdA15OTVGEP",
			SecretKey: "4885366DF30D880224FF6AA46FBDE4F0",
			PlayerCode: "4ucmqplxb029",
			OrderId: 1704777591586,
			Amount: 1m);

		var result = new DepositWithdrawResult(
			OrderNo: query.OrderId.ToString(),
			Balance: 1,
			Res: "{}");

		_ = fakeGameApiClient.DepositWithdrawAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			appId: Arg.Is(query.AppId),
			secretKey: Arg.Is(query.SecretKey),
			username: Arg.Is(query.PlayerCode),
			tradeNo: Arg.Is(query.OrderId),
			amount: Arg.Is(-query.Amount))
			.Returns(result);

		var sut = new FundInGetService(fakeGameApiClient);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(query.OrderId.ToString(), actual.TransactionId);
	}
}
