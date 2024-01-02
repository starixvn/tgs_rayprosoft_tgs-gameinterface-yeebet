using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class PlayerCreateCommandService(IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
    public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => client.RegisterAsync(
            command.ApiUrl,
            command.AppId,
            command.SecretKey,
            command.ClientType,
            command.Currency,
            command.LanguageId,
            command.PlayerCode,
            cancellationToken);
}
