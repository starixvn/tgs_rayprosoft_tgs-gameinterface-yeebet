using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class FundOutGetService : IGetService<FundOutQuery, FundResult>
{
    private readonly ILogger<FundOutGetService> _logger;
    private readonly ISnowflakeUtensil _snowflakeUtensil;
    private readonly IGameApiClient _client;

    public FundOutGetService(
        ILogger<FundOutGetService> logger,
        ISnowflakeUtensil snowflakeUtensil,
        IGameApiClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _snowflakeUtensil = snowflakeUtensil ?? throw new ArgumentNullException(nameof(snowflakeUtensil));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var trsId = _snowflakeUtensil.NextId();
        try
        {
            var result = await _client.DepositWithdrawAsync(
            apiUrl: query.ApiUrl,
            appId: query.AppId,
            secretKey: query.SecretKey,
            username: query.PlayerCode,
            tradeNo: trsId,
            amount: query.Amount,
            cancellationToken: cancellationToken).ConfigureAwait(false);

            return new FundResult(
                TransactionId: result.OrderNo,
                Balance: result.Balance,
                Response: result.Response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred with this fund out.");
            throw;
        }
    }
}
