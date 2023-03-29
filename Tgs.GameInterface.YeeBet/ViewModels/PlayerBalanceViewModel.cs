namespace Tgs.GameInterface.YeeBet.ViewModels;

public record PlayerBalanceViewModel(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode);
