namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

public record GameUrlQuery(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    int ClientType,
    int LanguageId,
    string PlayerCode);
