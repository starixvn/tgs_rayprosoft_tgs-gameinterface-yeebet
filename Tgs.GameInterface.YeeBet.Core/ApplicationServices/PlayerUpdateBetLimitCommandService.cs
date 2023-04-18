using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class PlayerUpdateBetLimitCommandService : IGetService<PlayerUpdateBetLimitCommand, bool>
{
    private readonly IGameApiClient _client;

    public PlayerUpdateBetLimitCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(PlayerUpdateBetLimitCommand query, CancellationToken cancellationToken = default)
        => _client.SetQuotaAsync(
            query.ApiUrl,
            query.AppId,
            query.SecretKey,
            query.PlayerCode,
            query.Quotas,
            cancellationToken);
}
