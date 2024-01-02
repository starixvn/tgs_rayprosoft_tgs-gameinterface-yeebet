using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class GameUrlGetService(IGameApiClient client) : IGetService<GameUrlQuery, string>
{
    public ValueTask<string> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
        => client.LoginAsync(
            apiUrl: query.ApiUrl,
            appId: query.AppId,
            secretKey: query.SecretKey,
            clientType: query.ClientType,
            languageId: query.LanguageId,
            username: query.PlayerCode,
            cancellationToken: cancellationToken);
}
