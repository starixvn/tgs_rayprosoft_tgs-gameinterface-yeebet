using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class PlayerBalanceGetService(IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client
            .BalanceAsync(query.ApiUrl, query.AppId, query.SecretKey, query.Username, cancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceResult(
            result.IsPlayerExist,
            result.Balance);
    }
}
