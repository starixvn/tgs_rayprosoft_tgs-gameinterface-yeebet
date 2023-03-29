namespace Tgs.GameInterface.YeeBet.ViewModels;

public record PlayerLogoutViewModel(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode);
