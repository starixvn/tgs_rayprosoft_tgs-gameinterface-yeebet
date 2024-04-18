using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class FundOutGetService(
	IGameApiClient client) : IGetService<FundOutQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.DepositWithdrawAsync(
			apiUrl: query.ApiUrl,
			appId: query.AppId,
			secretKey: query.SecretKey,
			username: query.PlayerCode,
			tradeNo: query.OrderId,
			amount: query.Amount,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new FundResult(
			TransactionId: result.OrderNo,
			Balance: result.Balance,
			Res: result.Res);
	}
}
