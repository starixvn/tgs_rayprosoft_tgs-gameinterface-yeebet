using Tgs.GameInterface.YeeBet.Core.Models;

namespace Tgs.GameInterface.YeeBet.Core;

public interface IGameApiClient
{
    public ValueTask<string> LoginAsync(Uri apiUrl, string appId, string secretKey, int clientType, int languageId, string username, CancellationToken cancellationToken = default);

    public ValueTask KickOutAsync(Uri apiUrl, string appId, string secretKey, string username, CancellationToken cancellationToken = default);

    public ValueTask RegisterAsync(Uri apiUrl, string appId, string secretKey, int clientType, string currency, int languageId, string username, CancellationToken cancellationToken = default);

    public ValueTask<BalanceResult> BalanceAsync(Uri apiUrl, string appId, string secretKey, string username, CancellationToken cancellationToken = default);

    public ValueTask<DepositWithdrawResult> DepositWithdrawAsync(Uri apiUrl, string appId, string secretKey, string username, long tradeNo, decimal amount, CancellationToken cancellationToken = default);

    public IAsyncEnumerable<BetResult> GetBetsAsync(Uri apiUrl, string appId, string secretKey, int size, CancellationToken cancellationToken = default);

    public ValueTask<bool> RemoveBetsAsync(Uri apiUrl, string appId, string secretKey, IEnumerable<string> externalRoundIds, CancellationToken cancellationToken = default);

    public IAsyncEnumerable<QuotaResult> QuotaAsync(Uri apiUrl, string appId, string secretKey, CancellationToken cancellationToken = default);

    public ValueTask<bool> DepositWithdrawHistoryAsync(Uri apiUrl, string appId, string secretKey, string username, string tradeno, CancellationToken cancellationToken = default);
}
