namespace Tgs.GameInterface.YeeBet.Core.Models;
public record BetResult(
    string ExternalRoundId,
    string ExternalGameId,
    string PlayerCode,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset ExternalClosedOn,
    decimal Bet,
    decimal ValidBet,
    decimal TurnOver,
    decimal WinLoss,
    string AdditionalData,
    bool IsVoid,
    bool IsClosed);
