namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;
public record PlayerUpdateBetLimitCommand(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode,
    IEnumerable<int> Quotas);
