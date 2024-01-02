using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class PlayerUpdateBetLimitCommandService(IGameApiClient client) : IGetService<PlayerUpdateBetLimitCommand, bool>
{
    public ValueTask<bool> GetAsync(PlayerUpdateBetLimitCommand query, CancellationToken cancellationToken = default)
        => client.SetQuotaAsync(
            query.ApiUrl,
            query.AppId,
            query.SecretKey,
            query.PlayerCode,
            query.Quotas,
            cancellationToken);
}
