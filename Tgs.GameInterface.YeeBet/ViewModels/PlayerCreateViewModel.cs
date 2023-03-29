namespace Tgs.GameInterface.YeeBet.ViewModels;

public record PlayerCreateViewModel(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode,
    int ClientType,
    string Currency,
    int LanguageId);
