namespace Tgs.GameInterface.YeeBet.ViewModels;

public record GameUrlQueryViewModel(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode,
    int ClientType,
    int LanguageId);
