using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class GameRecordMarkGetService(
	IGameApiClient client) : IGetService<GameRecordMarkQuery, bool>
{
	public ValueTask<bool> GetAsync(GameRecordMarkQuery query, CancellationToken cancellationToken = default)
		=> client.RemoveBetsAsync(
			apiUrl: query.ApiUrl,
			appId: query.AppId,
			secretKey: query.SecretKey,
			externalRoundIds: query.ExternalRoundIds,
			cancellationToken: cancellationToken);
}
