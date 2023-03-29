namespace Tgs.GameInterface.YeeBet.ViewModels;

public record FundStatusViewModel(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode,
    string TrsId);
