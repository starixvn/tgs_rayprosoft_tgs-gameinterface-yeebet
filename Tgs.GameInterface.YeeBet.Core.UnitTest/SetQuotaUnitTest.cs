using System.Net;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

public class SetQuotaUnitTest
{
	[Fact]
	public async Task 限額設置成功()
	{
		var apiUrl = new Uri("http://api.98kbus.top/");
		var appId = "xtdA15OTVGEP";
		var username = "4udw8w36hczi";
		var secretKey = "298511c51d785332d8d279998b79cd3c";
		var quotas = new int[] { 7, 8, 9 };

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHasher = Substitute.For<IYeeBetHasher>();

		var url = new Uri(apiUrl, $"/api/quota/set");

		var query = HttpUtility.ParseQueryString(string.Empty);
		query["appid"] = appId;
		query["username"] = username;
		query["qids"] = string.Join(",", quotas);

		var response = new
		{
			result = 0,
			desc = "succeed"
		};

		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();

		_ = handler.Expect(HttpMethod.Get, url.ToString())
			.WithQueryString(query.ToString()!)
			.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameApiClient(fakeLog, fakeHttpClient, fakeHasher);

		var actual = await sut.SetQuotaAsync(apiUrl, appId, secretKey, username, quotas);

		Assert.True(actual);
	}

	[Fact]
	public async Task 未註冊玩家限額設置失敗()
	{
		var apiUrl = new Uri("http://api.98kbus.top/");
		var appId = "xtdA15OTVGEP";
		var username = "4udxttua0glo";
		var secretKey = "298511c51d785332d8d279998b79cd3c";
		var quotas = new int[] { 7, 8, 9 };

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHasher = Substitute.For<IYeeBetHasher>();

		var url = new Uri(apiUrl, $"/api/quota/set");

		var query = HttpUtility.ParseQueryString(string.Empty);
		query["appid"] = appId;
		query["username"] = username;
		query["qids"] = string.Join(",", quotas);

		var response = new
		{
			result = -1001,
			desc = "server error!"
		};

		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();

		_ = handler.Expect(HttpMethod.Get, url.ToString())
			.WithQueryString(query.ToString()!)
			.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameApiClient(fakeLog, fakeHttpClient, fakeHasher);

		var actual = await sut.SetQuotaAsync(apiUrl, appId, secretKey, username, quotas);

		Assert.False(actual);
	}
}
