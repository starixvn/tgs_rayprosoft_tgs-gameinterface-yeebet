using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.YeeBet.Core.Models;

namespace Tgs.GameInterface.YeeBet.Core;

internal class GameApiClient(
    ILogger<GameApiClient> logger,
    HttpClient client,
    IYeeBetHasher hasher) : IGameApiClient
{
    public async ValueTask<BalanceResult> BalanceAsync(Uri apiUrl, string appId, string secretKey, string username, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
            { "username", username },
        };

        var sign = hasher.ComputeHash(parameters, secretKey);

        parameters.Add("sign", sign);

        var jsonNode = await GetAsync(apiUrl, path: "api/user/balance", parameters, "balance", cancellationToken).ConfigureAwait(false);

        var result = jsonNode!["result"]!.GetValue<int>();

        if (result < 0)
            return new BalanceResult(false, 0);

        var balance = jsonNode!["balance"]!.GetValue<decimal>();

        return new BalanceResult(true, balance);
    }

    public async ValueTask<BetsDetailResult> BetsDetailAsync(Uri apiUrl, string appId, string secretKey, DateTimeOffset beginTime, DateTimeOffset endTime, int pageSize, int pageIndex = 0, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
            { "begintime", beginTime.ToUnixTimeSeconds().ToString() },
            { "endtime", endTime.ToUnixTimeSeconds().ToString() },
            { "index", pageIndex.ToString() },
            { "size", pageSize.ToString() },
        };

        var sign = hasher.ComputeHash(parameters, secretKey);
        parameters.Add("sign", sign);

        var jsonNode = await GetAsync(apiUrl, "api/record/bets/detail", parameters, "betsdetail", cancellationToken).ConfigureAwait(false);

        var result = jsonNode["result"]!.GetValue<int>();

        if (result < 0)
            throw new Exception($"The game provider responded with an error.({result})");

        if (jsonNode["array"] is null)
        {
            logger.LogInformation("bets detail is null");
            return new BetsDetailResult
            {
                Total = 0,
                ArraySize = 0
            };
        }
        var array = jsonNode!["array"]!.AsArray();

        if (array is null || array.Count == 0)
        {
            return new BetsDetailResult
            {
                Total = 0,
                ArraySize = 0
            };
        }

        var total = jsonNode!["total"]!.GetValue<int>();
        var arraySize = jsonNode!["arraysize"]!.GetValue<int>();

        return new BetsDetailResult
        {
            Total = total,
            ArraySize = arraySize,
            Data = array.Select(item => new BetDetailResult(ExternalRoundId: item!["id"]!.ToString(),
                ExternalGameId: item!["gameid"]!.ToString(),
                PlayerCode: item!["username"]!.ToString(),
                ExternalCreatedOn: new DateTimeOffset(DateTime.Parse(item!["createtime"]!.ToString()), TimeSpan.FromHours(+8)),
                ExternalClosedOn: new DateTimeOffset(DateTime.Parse(item!["updatetime"]!.ToString()), TimeSpan.FromHours(+8)),
                Bet: item!["betamount"]!.GetValue<decimal>(),
                ValidBet: item!["commamount"]!.GetValue<decimal>(),
                TurnOver: item!["commamount"]!.GetValue<decimal>(),
                WinLoss: item!["winlost"]!.GetValue<decimal>(),
                AdditionalData: item.ToJsonString(),
                IsVoid: item!["state"]!.GetValue<int>() == -1,
                IsClosed: item!["state"]!.GetValue<int>() == 1))
        };
    }

    public async ValueTask<DepositWithdrawResult> DepositWithdrawAsync(Uri apiUrl, string appId, string secretKey, string username, long tradeNo, decimal amount, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
            { "username", username },
            { "amount", amount.ToString() },
            { "tradeno", tradeNo.ToString() },
        };

        var sign = hasher.ComputeHash(parameters, secretKey);
        parameters.Add("sign", sign);

        var jsonNode = await PostAsync(apiUrl, path: "api/user/dw", parameters, "depositwithdraw", cancellationToken).ConfigureAwait(false);

        var result = jsonNode["result"]!.GetValue<int>();

        if (result < 0)
            throw new Exception($"The game provider responded with an error.({result} {jsonNode["desc"]})");

        var orderNo = jsonNode!["orderno"]!.ToString();
        var balance = jsonNode!["balance"]!.GetValue<decimal>();

        return new DepositWithdrawResult(
            orderNo,
            balance,
            Res: jsonNode.ToString());
    }

    public async ValueTask<bool> DepositWithdrawHistoryAsync(Uri apiUrl, string appId, string secretKey, string username, long tradeno, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
            { "username",username },
            { "tradeno", tradeno.ToString() },
            { "size", "1" },
            { "index", "1" },
        };

        var sign = hasher.ComputeHash(parameters, secretKey);
        parameters.Add("sign", sign);

        var jsonNode = await GetAsync(apiUrl, "api/user/trade", parameters, "trade", cancellationToken).ConfigureAwait(false);

        var result = jsonNode["result"]!.GetValue<int>();

        if (result < 0)
            throw new Exception($"The game provider responded with an error.({result})");

        var total = jsonNode["total"]!.GetValue<int>();

        return total == 1;
    }

    public async IAsyncEnumerable<BetResult> GetBetsAsync(Uri apiUrl, string appId, string secretKey, int size, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
            { "size", size.ToString() },
        };

        var sign = hasher.ComputeHash(parameters, secretKey);
        parameters.Add("sign", sign);

        var jsonNode = await GetAsync(apiUrl, "api/data/getbets", parameters, "getbets", cancellationToken).ConfigureAwait(false);

        var result = jsonNode["result"]!.GetValue<int>();

        if (result < 0)
            throw new Exception($"The game provider responded with an error.({result})");

        if (jsonNode["array"] is null)
            yield break;

        var array = jsonNode!["array"]!.AsArray();

        if (array is null || array.Count == 0)
            yield break;

        foreach (var item in array)
        {
            var state = item!["state"]!.GetValue<int>();

            yield return new BetResult(
                ExternalRoundId: item!["id"]!.ToString(),
                ExternalGameId: item!["gameid"]!.ToString(),
                PlayerCode: item!["username"]!.ToString(),
                ExternalCreatedOn: DateTimeOffset.FromUnixTimeSeconds(item!["createtime"]!.GetValue<long>()),
                ExternalClosedOn: DateTimeOffset.FromUnixTimeSeconds(item!["settletime"]!.GetValue<long>()),
                Bet: item!["betamount"]!.GetValue<decimal>(),
                ValidBet: item!["commamount"]!.GetValue<decimal>(),
                TurnOver: item!["commamount"]!.GetValue<decimal>(),
                WinLoss: item!["winlost"]!.GetValue<decimal>(),
                AdditionalData: item.ToJsonString(),
                IsVoid: state == -1,
                IsClosed: state == 1);
        }
    }

    public async ValueTask KickOutAsync(Uri apiUrl, string appId, string secretKey, string username, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
            { "username", username },
        };

        var sign = hasher.ComputeHash(parameters, secretKey);
        parameters.Add("sign", sign);

        var jsonNode = await PostAsync(apiUrl, path: "api/kickout", parameters, "kickout", cancellationToken).ConfigureAwait(false);

        var result = jsonNode["result"]!.GetValue<int>();

        if (result < 0)
            throw new Exception($"The game provider responded with an error.({result} {jsonNode["desc"]})");
    }

    public async ValueTask<string> LoginAsync(Uri apiUrl, string appId, string secretKey, int clientType, int languageId, string username, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
            { "username", username },
            { "iscreate", "0" },
            { "clienttype", clientType.ToString() },
            { "language", languageId.ToString() },
        };

        var sign = hasher.ComputeHash(parameters, secretKey);

        parameters.Add("sign", sign);

        var jsonNode = await PostAsync(apiUrl, path: "api/login", parameters, "login", cancellationToken).ConfigureAwait(false);

        var result = jsonNode["result"]!.GetValue<int>();

        return result < 0
            ? throw new Exception($"The game provider responded with an error.({result} {jsonNode["desc"]})")
            : jsonNode!["openurl"]!.ToString();
    }

    public async IAsyncEnumerable<QuotaResult> QuotaAsync(Uri apiUrl, string appId, string secretKey, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
        };

        var sign = hasher.ComputeHash(parameters, secretKey);

        parameters.Add("sign", sign);

        var jsonNode = await GetAsync(apiUrl, "api/quota/list", parameters, "quotalist", cancellationToken).ConfigureAwait(false);

        var result = jsonNode["result"]!.GetValue<int>();

        if (result < 0)
            throw new Exception($"The game provider responded with an error.({result})");

        var array = jsonNode!["array"]!.AsArray();

        if (array is null || array.Count == 0)
            yield break;

        foreach (var item in array)
        {
            yield return new QuotaResult(
                item!["ID"]!.GetValue<int>(),
                item!["title"]!.ToString(),
                item!["up"]!.GetValue<decimal>(),
                item!["down"]!.GetValue<decimal>());
        }
    }

    public async ValueTask RegisterAsync(Uri apiUrl, string appId, string secretKey, int clientType, string currency, int languageId, string username, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
            { "username", username },
            { "iscreate", "1" },
            { "currency", currency },
            { "clienttype", clientType.ToString() },
            { "language", languageId.ToString() },
        };

        var sign = hasher.ComputeHash(parameters, secretKey);

        parameters.Add("sign", sign);

        var jsonNode = await PostAsync(apiUrl, path: "api/login", parameters, "register", cancellationToken).ConfigureAwait(false);

        var result = jsonNode["result"]!.GetValue<int>();

        if (result < 0)
            throw new Exception($"The game provider responded with an error.({result} {jsonNode["desc"]})");
    }

    public async ValueTask RegisterAsync(Uri apiUrl, string appId, string secretKey, int clientType, string currency, int languageId, IEnumerable<int> quotas, string username, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
            { "username", username },
            { "iscreate", "1" },
            { "currency", currency },
            { "clienttype", clientType.ToString() },
            { "language", languageId.ToString() },
        };

        if (quotas.Count() is not 0)
            parameters.Add("quotas", string.Join(",", quotas));

        var sign = hasher.ComputeHash(parameters, secretKey);

        parameters.Add("sign", sign);

        var jsonNode = await PostAsync(apiUrl, path: "api/login", parameters, "registerbyquota", cancellationToken).ConfigureAwait(false);

        var result = jsonNode["result"]!.GetValue<int>();

        if (result < 0)
            throw new Exception($"The game provider responded with an error.({result} {jsonNode["desc"]})");
    }

    public async ValueTask<bool> RemoveBetsAsync(Uri apiUrl, string appId, string secretKey, IEnumerable<string> externalRoundIds, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
            { "ids", string.Join(",",externalRoundIds) },
        };

        var sign = hasher.ComputeHash(parameters, secretKey);
        parameters.Add("sign", sign);

        var jsonNode = await GetAsync(apiUrl, "api/data/removebets", parameters, "removebets", cancellationToken).ConfigureAwait(false);

        var result = jsonNode!["result"]!.GetValue<int>();

        if (result < 0)
        {
            logger.LogError("The game provider responded with an error.({result} {desc})", result, jsonNode["desc"]!.ToString());
            return false;
        }

        return true;
    }

    public async ValueTask<bool> SetQuotaAsync(Uri apiUrl, string appId, string secretKey, string username, IEnumerable<int> quotas, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "appid", appId },
            { "username", username },
            { "qids", string.Join(",",quotas) },
        };

        var sign = hasher.ComputeHash(parameters, secretKey);
        parameters.Add("sign", sign);

        var jsonNode = await GetAsync(apiUrl, "api/quota/set", parameters, "setquota", cancellationToken).ConfigureAwait(false);

        var result = jsonNode!["result"]!.GetValue<int>();

        if (result < 0)
        {
            logger.LogError("The game provider responded with an error.({result} {desc})", result, jsonNode["desc"]!.ToString());
            return false;
        }

        return true;
    }

    private async ValueTask<JsonNode> GetAsync(Uri apiUrl, string path, IEnumerable<KeyValuePair<string, string>> parameters, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var queryStrings = HttpUtility.ParseQueryString(string.Empty);

        foreach (var kvp in parameters)
        {
            queryStrings.Add(kvp.Key, kvp.Value);
        }

        var sw = Stopwatch.StartNew();

        var url = new Uri(apiUrl, $"{path}?{queryStrings}");

        var httpResponseMessage = await client.GetAsync(url, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = string.Empty,
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage,
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };
        logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }

    private async ValueTask<JsonNode> PostAsync(Uri apiUrl, string path, IEnumerable<KeyValuePair<string, string>> parameters, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        HttpContent content = new FormUrlEncodedContent(parameters);

        var sw = Stopwatch.StartNew();

        var url = new Uri(apiUrl, path);

        var httpResponseMessage = await client.PostAsync(url, content, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };
        logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }
}
