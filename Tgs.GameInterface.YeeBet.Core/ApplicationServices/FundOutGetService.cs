using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class FundOutGetService(
    ILogger<FundOutGetService> logger,
    ISnowflakeUtensil snowflakeUtensil,
    IGameApiClient client) : IGetService<FundOutQuery, FundResult>
{
    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var trsId = snowflakeUtensil.NextId();
        try
        {
            var result = await client.DepositWithdrawAsync(
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
            logger.LogError(ex, "An exception occurred with this fund out.");
            throw;
        }
    }
}
