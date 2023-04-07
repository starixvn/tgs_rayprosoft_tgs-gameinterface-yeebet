using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class GameRecordInquireService : IQueryService<GameRecordInquire, GameRecordInfo>
{
    private readonly IGameApiClient _client;

    public GameRecordInquireService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameRecordInfo> QueryAsync(GameRecordInquire query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var bettingHistoryBets = await _client
            .BetsDetailAsync(
                apiUrl: query.ApiUrl,
                appId: query.AppId,
                secretKey: query.SecretKey,
                beginTime: query.BeginTime,
                endTime: query.EndTime,
                pageSize: query.PageSize,
                pageIndex: query.PageIndex,
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

            yield return new GameRecordInfo(
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
