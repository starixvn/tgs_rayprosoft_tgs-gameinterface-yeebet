namespace Tgs.GameInterface.YeeBet.ViewModels;

public record PlayerUpdateBetLimitViewModel(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode,
    IEnumerable<int> Quotas);
