namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

public record FundInQuery(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode,
    decimal Amount);
