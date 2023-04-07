using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class GameRecordInquireService : IGetService<GameRecordInquire, GameRecordInquireResult>
{
    private readonly IGameApiClient _client;

    public GameRecordInquireService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameRecordInquireResult> GetAsync(GameRecordInquire query, CancellationToken cancellationToken = default)
    {
        var betsResult = await _client
            .BetsDetailAsync(
                apiUrl: query.ApiUrl,
                appId: query.AppId,
                secretKey: query.SecretKey,
                beginTime: query.BeginTime,
                endTime: query.EndTime,
                pageSize: query.PageSize,
                pageIndex: query.PageIndex,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameRecordInquireResult
        {
            Total = betsResult.Total,
            ArraySize = betsResult.ArraySize,
            Data = betsResult.Data.Select(bettingHistoryBet => new GameRecordInfo(
                ExternalRoundId: bettingHistoryBet.ExternalRoundId,
                ExternalGameId: bettingHistoryBet.ExternalGameId,
                PlayerCode: bettingHistoryBet.PlayerCode,
                ExternalCreatedOn: bettingHistoryBet.ExternalCreatedOn,
                ExternalClosedOn: bettingHistoryBet.ExternalClosedOn,
                Bet: bettingHistoryBet.Bet,
                ValidBet: bettingHistoryBet.ValidBet,
                Settle: bettingHistoryBet.IsClosed && !bettingHistoryBet.IsVoid
                    ? bettingHistoryBet.WinLoss + bettingHistoryBet.Bet
                    : 0.0m,
                Turnover: bettingHistoryBet.ValidBet,
                PlayerWinLoss: bettingHistoryBet.WinLoss,
                AdditionalData: bettingHistoryBet.AdditionalData,
                IsClosed: bettingHistoryBet.IsClosed,
                IsVoid: bettingHistoryBet.IsVoid))
        };
    }
}
