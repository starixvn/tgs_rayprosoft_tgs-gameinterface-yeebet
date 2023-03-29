using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class GameRecordMarkGetService : IGetService<GameRecordMarkQuery, bool>
{
    private readonly IGameApiClient _client;

    public GameRecordMarkGetService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(GameRecordMarkQuery query, CancellationToken cancellationToken = default)
        => _client.RemoveBetsAsync(
            apiUrl: query.ApiUrl,
            appId: query.AppId,
            secretKey: query.SecretKey,
            externalRoundIds: query.ExternalRoundIds,
            cancellationToken: cancellationToken);
}
