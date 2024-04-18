using Microsoft.Extensions.Logging.Abstractions;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest;

public class YeeBetHasherUnitTest
{
	[Fact]
	public void Balance()
	{
		var fakeLog = NullLogger<YeeBetHasher>.Instance;

		var secretKey = "4885366DF30D880224FF6AA46FBDE4F0";

		var dict = new Dictionary<string, string>
		{
			{ "appid", "xtdA15OTVGEP" },
			{ "username", "4ucmqplxb029" }
		};

		var sut = new YeeBetHasher(fakeLog);

		var actual = sut.ComputeHash(dict, secretKey);

		Assert.Equal("298511c51d785332d8d279998b79cd3c", actual);
	}

	[Fact]
	public void Deposit()
	{
		var fakeLog = NullLogger<YeeBetHasher>.Instance;

		var secretKey = "4885366DF30D880224FF6AA46FBDE4F0";

		var dict = new Dictionary<string, string>
		{
			{ "appid", "xtdA15OTVGEP" },
			{ "username", "4ucmqplxb029" },
			{ "amount", 1000.ToString() },
			{ "tradeno", 1640520550106664960.ToString() }
		};

		var sut = new YeeBetHasher(fakeLog);

		var actual = sut.ComputeHash(dict, secretKey);

		Assert.Equal("7c410904c635f4681b10dace21a7b014", actual);
	}

	[Fact]
	public void Withdraw()
	{
		var fakeLog = NullLogger<YeeBetHasher>.Instance;

		var secretKey = "4885366DF30D880224FF6AA46FBDE4F0";

		var dict = new Dictionary<string, string>
		{
			{ "appid", "xtdA15OTVGEP" },
			{ "username", "4ucmqplxb029" },
			{ "amount", $"{-100}" },
			{ "tradeno", 1640526045735424000.ToString() }
		};

		var sut = new YeeBetHasher(fakeLog);

		var actual = sut.ComputeHash(dict, secretKey);

		Assert.Equal("b1989cd38e839a9c93158a89a003d9b8", actual);
	}

	[Fact]
	public void DepositWithdrawHistory()
	{
		var fakeLog = NullLogger<YeeBetHasher>.Instance;

		var secretKey = "4885366DF30D880224FF6AA46FBDE4F0";

		var dict = new Dictionary<string, string>
		{
			{ "appid", "xtdA15OTVGEP" },
			{ "username", "4ucmqplxb029" },
			{ "tradeno", 1640526045735424000.ToString() },
			{ "size", "1" },
			{ "index", "1" }
		};

		var sut = new YeeBetHasher(fakeLog);

		var actual = sut.ComputeHash(dict, secretKey);

		Assert.Equal("265df11a021a872e86399265f4af6b2c", actual);
	}

	[Fact]
	public void GetBets()
	{
		var fakeLog = NullLogger<YeeBetHasher>.Instance;

		var secretKey = "4885366DF30D880224FF6AA46FBDE4F0";

		var dict = new Dictionary<string, string>
		{
			{ "appid", "xtdA15OTVGEP" },
			{ "size", "1000" },
		};

		var sut = new YeeBetHasher(fakeLog);

		var actual = sut.ComputeHash(dict, secretKey);

		Assert.Equal("192f7ba5aab1da971b91556ce2ba8784", actual);
	}

	[Fact]
	public void Kickout()
	{
		var fakeLog = NullLogger<YeeBetHasher>.Instance;

		var secretKey = "4885366DF30D880224FF6AA46FBDE4F0";

		var dict = new Dictionary<string, string>
		{
			{ "appid", "xtdA15OTVGEP" },
			{ "username", "4ucmqplxkuyd" },
		};

		var sut = new YeeBetHasher(fakeLog);

		var actual = sut.ComputeHash(dict, secretKey);

		Assert.Equal("fcf19903e7959335c56046c0e0df20a4", actual);
	}

	[Fact]
	public void Login()
	{
		var fakeLog = NullLogger<YeeBetHasher>.Instance;

		var secretKey = "4885366DF30D880224FF6AA46FBDE4F0";

		var dict = new Dictionary<string, string>
		{
			{ "appid", "xtdA15OTVGEP" },
			{ "username", "4ucmqplxkuyd" },
			{ "iscreate","0" },
			{ "clienttype","1" },
			{ "language","2" },
		};

		var sut = new YeeBetHasher(fakeLog);

		var actual = sut.ComputeHash(dict, secretKey);

		Assert.Equal("160a478852a780e92d338615b52b8f12", actual);
	}

	[Fact]
	public void Quota()
	{
		var fakeLog = NullLogger<YeeBetHasher>.Instance;

		var secretKey = "4885366DF30D880224FF6AA46FBDE4F0";

		var dict = new Dictionary<string, string>
		{
			{ "appid", "xtdA15OTVGEP" },
		};

		var sut = new YeeBetHasher(fakeLog);

		var actual = sut.ComputeHash(dict, secretKey);

		Assert.Equal("115263331acd6896865a42beef8c710e", actual);
	}

	[Fact]
	public void Register()
	{
		var fakeLog = NullLogger<YeeBetHasher>.Instance;

		var secretKey = "4885366DF30D880224FF6AA46FBDE4F0";

		var dict = new Dictionary<string, string>
		{
			{ "appid", "xtdA15OTVGEP" },
			{ "username", "4ucmqplxkuyd" },
			{ "iscreate","1" },
			{ "currency","CNY" },
			{ "clienttype","1" },
			{ "language","1" },
		};

		var sut = new YeeBetHasher(fakeLog);

		var actual = sut.ComputeHash(dict, secretKey);

		Assert.Equal("e9d5dfbbb6e100bd92fc2bdb3710ad40", actual);
	}

	[Fact]
	public void RemoveBets()
	{
		var fakeLog = NullLogger<YeeBetHasher>.Instance;

		var secretKey = "4885366DF30D880224FF6AA46FBDE4F0";

		var dict = new Dictionary<string, string>
		{
			{ "appid", "xtdA15OTVGEP" },
			{ "ids", "889025" },
		};

		var sut = new YeeBetHasher(fakeLog);

		var actual = sut.ComputeHash(dict, secretKey);

		Assert.Equal("306b39a751e9972416992204a7d24a03", actual);
	}
}
