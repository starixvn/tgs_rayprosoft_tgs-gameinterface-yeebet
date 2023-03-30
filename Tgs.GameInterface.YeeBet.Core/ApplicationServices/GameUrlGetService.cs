using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, string>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<string> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
        => _client.LoginAsync(
            apiUrl: query.ApiUrl,
            appId: query.AppId,
            secretKey: query.SecretKey,
            clientType:query.ClientType,            
            languageId: query.LanguageId,
            username: query.PlayerCode,
            cancellationToken: cancellationToken);
}
