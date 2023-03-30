namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

public record PlayerLogoutCommand(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode);
