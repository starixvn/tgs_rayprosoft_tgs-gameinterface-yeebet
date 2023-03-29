namespace Tgs.GameInterface.YeeBet.ViewModels;

public record FundResultViewModel(
    string TransactionId,
    decimal Balance,    
    string Response);
