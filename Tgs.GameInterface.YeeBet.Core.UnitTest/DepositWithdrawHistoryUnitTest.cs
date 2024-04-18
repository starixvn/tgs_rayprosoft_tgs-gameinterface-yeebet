using System.Net;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

public class DepositWithdrawHistoryUnitTest
{
	[Fact]
	public async Task 有存取款紀錄()
	{
		var apiUrl = new Uri("http://api.98kbus.top/");
		var appId = "xtdA15OTVGEP";
		var secretKey = "298511c51d785332d8d279998b79cd3c";
		var username = "4ucmqplxb029";
		var tradeNo = 1640526045735424000;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHasher = Substitute.For<IYeeBetHasher>();

		var url = new Uri(apiUrl, $"/api/user/trade");

		var query = HttpUtility.ParseQueryString(string.Empty);
		query["appid"] = appId;
		query["username"] = username;
		query["tradeno"] = tradeNo.ToString();
		query["size"] = 1.ToString();
		query["index"] = 1.ToString();

		var response = new
		{
			result = 0,
			desc = "succeed",
			total = 1,
			data = Array.Empty<object>()
		};

		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();

		_ = handler.Expect(HttpMethod.Get, url.ToString())
			.WithQueryString(query.ToString()!)
			.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameApiClient(fakeLog, fakeHttpClient, fakeHasher);

		var actual = await sut.DepositWithdrawHistoryAsync(apiUrl, appId, secretKey, username, tradeNo);

		Assert.True(actual);
	}

	[Fact]
	public async Task 無存取款紀錄()
	{
		var apiUrl = new Uri("http://api.98kbus.top/");
		var appId = "xtdA15OTVGEP";
		var secretKey = "298511c51d785332d8d279998b79cd3c";
		var username = "4ucmqplxb029";
		var tradeNo = 1234;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHasher = Substitute.For<IYeeBetHasher>();

		var url = new Uri(apiUrl, $"/api/user/trade");

		var query = HttpUtility.ParseQueryString(string.Empty);
		query["appid"] = appId;
		query["username"] = username;
		query["tradeno"] = tradeNo.ToString();
		query["size"] = 1.ToString();
		query["index"] = 1.ToString();

		var response = new
		{
			result = 0,
			desc = "succeed",
			total = 0,
			data = Array.Empty<object>()
		};

		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();

		_ = handler.Expect(HttpMethod.Get, url.ToString())
			.WithQueryString(query.ToString()!)
			.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameApiClient(fakeLog, fakeHttpClient, fakeHasher);

		var actual = await sut.DepositWithdrawHistoryAsync(apiUrl, appId, secretKey, username, tradeNo);

		Assert.False(actual);
	}
}
