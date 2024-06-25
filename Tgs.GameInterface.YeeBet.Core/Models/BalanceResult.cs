namespace Tgs.GameInterface.YeeBet.Core.Models;
public record BalanceResult(
	bool IsPlayerExist,
	decimal Balance,
	bool Isonline,
	bool Isplaying);
