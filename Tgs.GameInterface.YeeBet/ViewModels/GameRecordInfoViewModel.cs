namespace Tgs.GameInterface.YeeBet.ViewModels;

public record GameRecordInfoViewModel(
	string ExternalRoundId,
	string ExternalGameId,
	string PlayerCode,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal Turnover,
	decimal WinLoss,
	string AdditionalData,
	bool IsClosed,
	bool IsVoid);
