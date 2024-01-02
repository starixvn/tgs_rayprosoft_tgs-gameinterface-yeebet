using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class GameRecordQueryService(
    IGameApiClient client) : IQueryService<GameRecordQuery, GameRecordResult>
{
    public async IAsyncEnumerable<GameRecordResult> QueryAsync(GameRecordQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var bettingHistoryBets = await client
            .GetBetsAsync(
                apiUrl: query.ApiUrl,
                appId: query.AppId,
                secretKey: query.SecretKey,
                size: query.Size,
                cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);

        if (bettingHistoryBets.Length == 0)
        {
            yield break;
        }

        foreach (var bettingHistoryBet in bettingHistoryBets)
        {
            var Settle = 0.0m;
            if (bettingHistoryBet.IsClosed && !bettingHistoryBet.IsVoid)
            {
                Settle = bettingHistoryBet.WinLoss + bettingHistoryBet.Bet;
            }

            yield return new GameRecordResult(
                ExternalRoundId: bettingHistoryBet.ExternalRoundId,
                ExternalGameId: bettingHistoryBet.ExternalGameId,
                PlayerCode: bettingHistoryBet.PlayerCode,
                ExternalCreatedOn: bettingHistoryBet.ExternalCreatedOn,
                ExternalClosedOn: bettingHistoryBet.ExternalClosedOn,
                Bet: bettingHistoryBet.Bet,
                ValidBet: bettingHistoryBet.ValidBet,
                Settle: Settle,
                Turnover: bettingHistoryBet.ValidBet,
                PlayerWinLoss: bettingHistoryBet.WinLoss,
                AdditionalData: bettingHistoryBet.AdditionalData,
                IsClosed: bettingHistoryBet.IsClosed,
                IsVoid: bettingHistoryBet.IsVoid);
        }
    }
}
