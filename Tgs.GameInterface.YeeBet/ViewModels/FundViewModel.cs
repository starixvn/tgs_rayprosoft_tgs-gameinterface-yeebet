namespace Tgs.GameInterface.YeeBet.ViewModels;

public record FundViewModel(
	Uri ApiUrl,
	string AppId,
	string SecretKey,
	string PlayerCode,
	long OrderId,
	decimal Amount);
