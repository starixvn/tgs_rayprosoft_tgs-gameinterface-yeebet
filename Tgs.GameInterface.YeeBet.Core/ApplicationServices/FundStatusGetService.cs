using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class FundStatusGetService(IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
	public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
		=> client.DepositWithdrawHistoryAsync(
				apiUrl: query.ApiUrl,
				appId: query.AppId,
				secretKey: query.SecretKey,
				username: query.PlayerCode,
				tradeno: query.OrderId,
				cancellationToken: cancellationToken);
}
