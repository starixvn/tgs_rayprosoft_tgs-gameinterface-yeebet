using Tgs.GameInterface.YeeBet.Protos;

namespace Tgs.GameInterface.YeeBet.Client;

public interface IGameInterfaceYeeBetClient
{
    ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string appId,
        string secretKey,
        string playerCode,
        int clientType,
        string currency,
        int languageId,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
       Uri apiUrl,
       string appId,
       string secretKey,
       string playerCode,
       CancellationToken cancellationToken = default);

    ValueTask PlayerLogoutAsync(
        Uri apiUrl,
        string appId,
        string secretKey,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string appId,
        string secretKey,
        string playerCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string appId,
        string secretKey,
        string playerCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string appId,
        string secretKey,
        string playerCode,
        string trsId,
        CancellationToken cancellationToken = default);

    ValueTask<string> GameUrlAsync(
        Uri apiUrl,
        string appId,
        string secretKey,
        string playerCode,
        int clientType,
        int languageId,
      CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameRecordInfo> GameRecordAsync(
       Uri apiUrl,
       string appId,
       string secretKey,
       int size,
       CancellationToken cancellationToken = default);

    ValueTask<BaseReply> GameRecordMarkAsync(
        Uri apiUrl,
        string appId,
        string secretKey,
        IEnumerable<string> externalRoundIds,
        CancellationToken cancellationToken = default);
}
