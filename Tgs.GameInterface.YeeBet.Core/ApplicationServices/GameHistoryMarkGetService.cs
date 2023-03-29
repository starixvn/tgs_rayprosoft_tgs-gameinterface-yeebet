using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class GameHistoryMarkGetService : IGetService<GameHistoryMarkQuery, bool>
{
    private readonly IGameApiClient _client;

    public GameHistoryMarkGetService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(GameHistoryMarkQuery query, CancellationToken cancellationToken = default)
        => _client.RemoveBetsAsync(
            apiUrl: query.ApiUrl,
            appId: query.AppId,
            secretKey: query.SecretKey,
            externalRoundIds: query.ExternalRoundIds,
            cancellationToken: cancellationToken);
}
