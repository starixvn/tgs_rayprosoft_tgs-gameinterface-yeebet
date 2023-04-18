namespace Tgs.GameInterface.YeeBet.ViewModels;

public record PlayerCreateByBetLimitViewModel(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode,
    int ClientType,
    string Currency,
    int LanguageId,
    IEnumerable<int> Quotas);
