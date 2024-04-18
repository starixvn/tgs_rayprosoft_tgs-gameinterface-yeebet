using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.YeeBet.Protos;

namespace Tgs.GameInterface.YeeBet.Client;

internal class GameInterfaceYeeBetClient(
	GameInterfaceYeeBet.GameInterfaceYeeBetClient client) : IGameInterfaceYeeBetClient
{
	public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
	{
		var request = new CheckingServiceRequest
		{
			MachineName = machineName,
		};

		var reply = await client
			.CheckingServiceAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.MachineName;
	}

	public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string appId, string secretKey, string playerCode, long orderId, decimal amount, CancellationToken cancellationToken = default)
		=> await client.FundInAsync(
			request: new FundRequest
			{
				ApiUrl = apiUrl.ToString(),
				AppId = appId,
				SecretKey = secretKey,
				PlayerCode = playerCode,
				OrderId = orderId,
				Amount = amount
			},
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

	public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string appId, string secretKey, string playerCode, long orderId, decimal amount, CancellationToken cancellationToken = default)
		=> await client.FundOutAsync(
			request: new FundRequest
			{
				ApiUrl = apiUrl.ToString(),
				AppId = appId,
				SecretKey = secretKey,
				PlayerCode = playerCode,
				OrderId = orderId,
				Amount = amount,
			},
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

	public async ValueTask<bool> FundStatusAsync(Uri apiUrl, string appId, string secretKey, string playerCode, long orderId, CancellationToken cancellationToken = default)
	{
		var reply = await client
		.FundStatusAsync(
			request: new FundStatusRequest
			{
				ApiUrl = apiUrl.ToString(),
				AppId = appId,
				SecretKey = secretKey,
				PlayerCode = playerCode,
				OrderId = orderId,
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

		return reply.State;
	}

	public async ValueTask<GameRecordInfoReply> GameRecordInquireAsync(Uri apiUrl, string appId, string secretKey, DateTimeOffset beginTime, DateTimeOffset endTime, int pageSize, int pageIndex, CancellationToken cancellationToken = default)
	{
		var reply = await client
			.GameRecordInquireAsync(
				request: new GameRecordInquireRequest
				{
					ApiUrl = apiUrl.ToString(),
					AppId = appId,
					SecretKey = secretKey,
					BeginTime = beginTime.ToTimestamp(),
					EndTime = endTime.ToTimestamp(),
					PageIndex = pageIndex,
					PageSize = pageSize,
				},
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply;
	}

	public async IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri apiUrl, string appId, string secretKey, int size, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var reply = await client
			.GameRecordAsync(
				request: new GameRecordRequest
				{
					ApiUrl = apiUrl.ToString(),
					AppId = appId,
					SecretKey = secretKey,
					Size = size,
				},
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var gameRecords = reply.Items
			?? Enumerable.Empty<GameRecordInfo>();

		if (!gameRecords.Any())
		{
			yield break;
		}

		foreach (var gameRecord in gameRecords)
		{
			yield return gameRecord;
		}
	}

	public async ValueTask<BaseReply> GameRecordMarkAsync(Uri apiUrl, string appId, string secretKey, IEnumerable<string> externalRoundIds, CancellationToken cancellationToken = default)
	{
		var request = new GameRecordMarkRequest
		{
			ApiUrl = apiUrl.ToString(),
			AppId = appId,
			SecretKey = secretKey,
		};

		foreach (var item in externalRoundIds)
		{
			request.ExternalRoundIds.Add(item);
		}

		return await client
			.GameRecordMarkAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<string> GameUrlAsync(Uri apiUrl, string appId, string secretKey, string playerCode, int clientType, int languageId, CancellationToken cancellationToken = default)
	{
		var result = await client
			.GameUrlAsync(
				request: new GameUrlRequest
				{
					ApiUrl = apiUrl.ToString(),
					AppId = appId,
					SecretKey = secretKey,
					PlayerCode = playerCode,
					ClientType = clientType,
					LanguageId = languageId,
				},
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return result.GameUrl;
	}

	public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string appId, string secretKey, string playerCode, CancellationToken cancellationToken = default)
		=> await client
			.PlayerBalanceAsync(
				request: new PlayerBalanceRequest
				{
					ApiUrl = apiUrl.ToString(),
					AppId = appId,
					SecretKey = secretKey,
					PlayerCode = playerCode,
				},
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

	public async ValueTask PlayerCreateAsync(Uri apiUrl, string appId, string secretKey, string playerCode, int clientType, string currency, int languageId, CancellationToken cancellationToken = default)
	{
		var request = new PlayerCreateRequest
		{
			ApiUrl = apiUrl.ToString(),
			AppId = appId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
			ClientType = clientType,
			Currency = currency,
			LanguageId = languageId,
		};

		_ = await client
			.PlayerCreateAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask PlayerCreateByBetLimitAsync(Uri apiUrl, string appId, string secretKey, string playerCode, int clientType, string currency, int languageId, IEnumerable<int> quotas, CancellationToken cancellationToken = default)
	{
		var request = new PlayerCreateByBetLimitRequest
		{
			ApiUrl = apiUrl.ToString(),
			AppId = appId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
			ClientType = clientType,
			Currency = currency,
			LanguageId = languageId,
		};

		if (quotas.Any())
		{
			foreach (var quota in quotas)
			{
				request.Quotas.Add(quota);
			}
		}

		_ = await client
			.PlayerCreateByBetLimitAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask PlayerLogoutAsync(Uri apiUrl, string appId, string secretKey, string playerCode, CancellationToken cancellationToken = default)
		=> await client
			.PlayerLogoutAsync(
				request: new PlayerLogoutRequest
				{
					ApiUrl = apiUrl.ToString(),
					AppId = appId,
					SecretKey = secretKey,
					PlayerCode = playerCode,
				},
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

	public async ValueTask<bool> PlayerUpdateBetLimitAsync(Uri apiUrl, string appId, string secretKey, string playerCode, IEnumerable<int> quotas, CancellationToken cancellationToken = default)
	{
		var request = new PlayerUpdateBetLimitRequest
		{
			ApiUrl = apiUrl.ToString(),
			AppId = appId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
		};

		foreach (var quota in quotas)
		{
			request.Quotas.Add(quota);
		}

		var reply = await client
			.PlayerUpdateBetLimitAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.State;
	}
}
