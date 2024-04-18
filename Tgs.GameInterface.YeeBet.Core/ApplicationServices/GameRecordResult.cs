namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

public record GameRecordResult(
	string PlayerCode,
	string ExternalRoundId,
	bool IsClosed,
	bool IsVoid,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal ValidBet,
	decimal PlayerWinLoss,
	decimal Turnover,
	decimal Settle,
	string AdditionalData,
	string ExternalGameId);
