namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

public record FundOutQuery(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    string PlayerCode,
    long OrderId,
    decimal Amount);
