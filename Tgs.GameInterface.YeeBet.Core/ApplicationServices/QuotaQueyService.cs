using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

internal class QuotaQueyService : IQueryService<QuotaQuery, QuotaInfo>
{
    private readonly IGameApiClient _client;

    public QuotaQueyService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<QuotaInfo> QueryAsync(QuotaQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var quotas = await _client
            .QuotaAsync(
                apiUrl: query.ApiUrl,
                appId: query.AppId,
                secretKey: query.SecretKey,
                cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!quotas.Any())
        {
            yield break;
        }

        foreach (var quota in quotas)
        {
            yield return new QuotaInfo(
                Id: quota.Id,
                Title: quota.Title,
                Up: quota.Up,
                Down: quota.Down);
        }
    }
}
