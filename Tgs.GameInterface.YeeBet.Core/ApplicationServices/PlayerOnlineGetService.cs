using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class PlayerOnlineGetService(
	IGameApiClient client) : IGetService<PlayerOnlineQuery, bool>
{
	public async ValueTask<bool> GetAsync(PlayerOnlineQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.BalanceAsync(
			query.ApiUrl,
			query.AppId,
			query.SecretKey,
			query.Username,
			cancellationToken)
			.ConfigureAwait(false);

		return result.IsPlayerExist && result.Isonline;
	}
}
