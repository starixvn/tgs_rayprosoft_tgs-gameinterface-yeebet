using Tgs.GameInterface.YeeBet.Protos;

namespace Tgs.GameInterface.YeeBet.Client;

public interface IGameInterfaceYeeBetClient
{
    ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

    ValueTask PlayerCreateAsync(Uri apiUrl, string appId, string secretKey, string playerCode, int clientType, string currency, int languageId, CancellationToken cancellationToken = default);

    ValueTask PlayerCreateByBetLimitAsync(Uri apiUrl, string appId, string secretKey, string playerCode, int clientType, string currency, int languageId, IEnumerable<int> quotas, CancellationToken cancellationToken = default);

    ValueTask<bool> PlayerUpdateBetLimitAsync(Uri apiUrl, string appId, string secretKey, string playerCode, IEnumerable<int> quotas, CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string appId, string secretKey, string playerCode, CancellationToken cancellationToken = default);

    ValueTask PlayerLogoutAsync(Uri apiUrl, string appId, string secretKey, string playerCode, CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(Uri apiUrl, string appId, string secretKey, string playerCode, long orderId, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(Uri apiUrl, string appId, string secretKey, string playerCode, long orderId, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(Uri apiUrl, string appId, string secretKey, string playerCode, long orderId, CancellationToken cancellationToken = default);

    ValueTask<string> GameUrlAsync(Uri apiUrl, string appId, string secretKey, string playerCode, int clientType, int languageId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri apiUrl, string appId, string secretKey, int size, CancellationToken cancellationToken = default);

    ValueTask<BaseReply> GameRecordMarkAsync(Uri apiUrl, string appId, string secretKey, IEnumerable<string> externalRoundIds, CancellationToken cancellationToken = default);

    ValueTask<GameRecordInfoReply> GameRecordInquireAsync(Uri apiUrl, string appId, string secretKey, DateTimeOffset beginTime, DateTimeOffset endTime, int pageSize, int pageIndex, CancellationToken cancellationToken = default);
}
