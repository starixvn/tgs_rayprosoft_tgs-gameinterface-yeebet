using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class PlayerCreateByBetLimitCommandService(IGameApiClient client) : ICommandService<PlayerCreateByBetLimitCommand>
{
	public ValueTask ExecuteAsync(PlayerCreateByBetLimitCommand command, CancellationToken cancellationToken = default)
		=> client.RegisterAsync(
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
