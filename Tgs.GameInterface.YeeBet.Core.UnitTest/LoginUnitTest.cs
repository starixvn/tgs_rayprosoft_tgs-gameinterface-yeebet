using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

public class LoginUnitTest
{
	[Fact]
	public async Task 登入遊戲成功()
	{
		var apiUrl = new Uri("http://api.98kbus.top/");
		var appId = "xtdA15OTVGEP";
		var secretKey = "298511c51d785332d8d279998b79cd3c";
		var clientType = 1;//pc
		var languageId = 2;
		var username = "4ucmqplxkuyd";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHasher = Substitute.For<IYeeBetHasher>();

		var url = new Uri(apiUrl, $"/api/login");

		var dict = new Dictionary<string, string>
		{
			["appid"] = appId,
			["username"] = username,
			["iscreate"] = 0.ToString(),
			["clienttype"] = clientType.ToString(),
			["language"] = languageId.ToString(),
		};

		var gameUrl = "http://yibo.98kbus.top/direct/login/MTY3OTk3MDc0Ml8zMDBfMzI5NTY4XzNBM0YyNQ==?d=5&l=2";

		var response = new
		{
			result = 0,
			desc = "succeed",
			openurl = gameUrl
		};

		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();

		_ = handler.Expect(HttpMethod.Post, url.ToString())
			.With(req => req.Content?.Headers.ContentType?.MediaType == "application/x-www-form-urlencoded")
			.WithFormData(dict)
			.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameApiClient(fakeLog, fakeHttpClient, fakeHasher);

		var actual = await sut.LoginAsync(apiUrl, appId, secretKey, clientType, languageId, username);

		Assert.Equal(gameUrl, actual);
	}

	[Fact]
	public async Task 未註冊玩家登入遊戲失敗()
	{
		var apiUrl = new Uri("http://api.98kbus.top/");
		var appId = "xtdA15OTVGEP";
		var secretKey = "298511c51d785332d8d279998b79cd3c";
		var clientType = 1;
		var languageId = 1;
		var username = "4udxttua0glo";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHasher = Substitute.For<IYeeBetHasher>();

		var url = new Uri(apiUrl, $"/api/login");

		var dict = new Dictionary<string, string>
		{
			["appid"] = appId,
			["username"] = username,
			["iscreate"] = 0.ToString(),
			["clienttype"] = clientType.ToString(),
			["language"] = languageId.ToString(),
		};

		string? gameUrl = null;

		var response = new
		{
			result = -1011,
			desc = "unknown error!",
			openurl = gameUrl
		};

		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();

		_ = handler.Expect(HttpMethod.Post, url.ToString())
			.With(req => req.Content?.Headers.ContentType?.MediaType == "application/x-www-form-urlencoded")
			.WithFormData(dict)
			.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameApiClient(fakeLog, fakeHttpClient, fakeHasher);

		_ = await Assert.ThrowsAsync<Exception>(() => sut
			.LoginAsync(apiUrl, appId, secretKey, clientType, languageId, username)
			.AsTask());
	}
}
