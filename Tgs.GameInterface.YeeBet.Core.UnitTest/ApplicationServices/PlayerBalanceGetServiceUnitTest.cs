using NSubstitute;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;
using Tgs.GameInterface.YeeBet.Core.Models;

namespace Tgs.GameInterface.YeeBet.Core.UnitTest.ApplicationServices;

[TestClass]
public class PlayerBalanceGetServiceUnitTest
{
    [TestMethod]
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
            .Returns(new BalanceResult(true, Balance: 11000.0M));

        var sut = new PlayerBalanceGetService(fakeGameApiClient);

        var actual = await sut
            .GetAsync(query)
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.IsPlayerExist);
        Assert.AreEqual(11000.0M, actual.Balance);
    }
}
