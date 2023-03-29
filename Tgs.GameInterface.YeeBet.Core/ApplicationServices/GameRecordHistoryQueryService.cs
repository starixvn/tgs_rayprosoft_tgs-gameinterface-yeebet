using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class GameRecordHistoryQueryService : IQueryService<GameRecordHistoryQuery, GameRecordResult>
{
    private readonly IGameApiClient _client;

    public GameRecordHistoryQueryService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameRecordResult> QueryAsync(GameRecordHistoryQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var bettingHistoryBets = await _client
            .GetBetsAsync(
                apiUrl: query.ApiUrl,
                appId: query.AppId,
                secretKey: query.SecretKey,
                size: query.Size,
                cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!bettingHistoryBets.Any())
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
                PlayerCode: bettingHistoryBet.PlayerCode,
                ExternalRoundId: bettingHistoryBet.ExternalRoundId,
                IsClosed: bettingHistoryBet.IsClosed,
                IsVoid: bettingHistoryBet.IsVoid,
                ExternalCreatedOn: bettingHistoryBet.ExternalCreatedOn,
                ExternalClosedOn: bettingHistoryBet.ExternalClosedOn,
                Bet: bettingHistoryBet.Bet,
                ValidBet: bettingHistoryBet.ValidBet,
                PlayerWinLoss: bettingHistoryBet.WinLoss,
                Turnover: bettingHistoryBet.ValidBet,
                Settle: Settle,
                AdditionalData: bettingHistoryBet.AdditionalData,
                ExternalGameId: bettingHistoryBet.ExternalGameId);
        }
    }
}
