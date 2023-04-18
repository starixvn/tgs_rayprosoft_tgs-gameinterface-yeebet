using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class PlayerCreateByBetLimitCommandService : ICommandService<PlayerCreateByBetLimitCommand>
{
    private readonly IGameApiClient _client;

    public PlayerCreateByBetLimitCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(PlayerCreateByBetLimitCommand command, CancellationToken cancellationToken = default)
        => _client.RegisterAsync(
            command.ApiUrl,
            command.AppId,
            command.SecretKey,
            command.ClientType,
            command.Currency,
            command.LanguageId,
            command.Quotas,
            command.PlayerCode,
            cancellationToken);
}
