namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;
public record PlayerCreateByBetLimitCommand(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode,
    int ClientType,
    string Currency,
    int LanguageId,
    IEnumerable<int> Quotas);
