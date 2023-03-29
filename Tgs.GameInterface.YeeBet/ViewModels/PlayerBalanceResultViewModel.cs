namespace Tgs.GameInterface.YeeBet.ViewModels;

public record PlayerBalanceResultViewModel(
    bool IsPlayerExist,
    decimal? Balance);