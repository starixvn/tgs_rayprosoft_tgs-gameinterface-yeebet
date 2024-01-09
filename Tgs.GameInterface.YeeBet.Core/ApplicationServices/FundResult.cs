namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

public record FundResult(
    string TransactionId,
    decimal Balance,
    string Res);
