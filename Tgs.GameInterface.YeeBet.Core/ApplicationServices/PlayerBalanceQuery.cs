namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;
public record PlayerBalanceQuery(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string Username);
