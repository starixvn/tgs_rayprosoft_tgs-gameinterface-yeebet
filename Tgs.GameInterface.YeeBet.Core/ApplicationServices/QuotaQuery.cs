namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;
public record QuotaQuery(
    Uri ApiUrl,
    string AppId,
    string SecretKey);
