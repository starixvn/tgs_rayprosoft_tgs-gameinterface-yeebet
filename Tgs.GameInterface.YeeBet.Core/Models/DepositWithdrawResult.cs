namespace Tgs.GameInterface.YeeBet.Core.Models;
public record DepositWithdrawResult(
    string OrderNo,
    decimal Balance,
    string Response);
