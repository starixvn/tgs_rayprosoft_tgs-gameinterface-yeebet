using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;
using Tgs.GameInterface.YeeBet.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.GrpcServices;

public class GameInterfaceYeeBetGrpcService(
    ILogger<GameInterfaceYeeBetGrpcService> logger,
    IGetService<FundOutQuery, FundResult> fundOutService,
    IGetService<FundInQuery, FundResult> fundInService,
    IGetService<FundStatusQuery, bool> fundStatusService,
    IGetService<GameUrlQuery, string> gameUrlService,
    ICommandService<PlayerCreateCommand> playerCreateService,
    ICommandService<PlayerCreateByBetLimitCommand> playerCreateByBetLimitService,
    IGetService<PlayerUpdateBetLimitCommand, bool> playerUpdateBetLimitService,
    ICommandService<PlayerLogoutCommand> playerLogoutService,
    IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
    IQueryService<GameRecordQuery, GameRecordResult> gameRecordService,
    IGetService<GameRecordMarkQuery, bool> gameRecordMarkService,
    IGetService<GameRecordInquire, GameRecordInquireResult> gameRecordInquireService) : GameInterfaceYeeBet.GameInterfaceYeeBetBase
{
    public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
    {
        logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

        return Task.FromResult(new CheckingServiceReply
        {
            MachineName = Environment.MachineName
        });
    }

    public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
    {
        var query = new FundOutQuery(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.PlayerCode,
            request.Amount);

        var result = await fundOutService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply
        {
            Balance = result.Balance,
            TransactionId = result.TransactionId,
            Response = result.Response,
        };
    }

    public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
    {
        var query = new FundInQuery(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.PlayerCode,
            request.Amount);

        var result = await fundInService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply
        {
            Balance = result.Balance,
            TransactionId = result.TransactionId,
            Response = result.Response,
        };
    }

    public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
    {
        var query = new FundStatusQuery(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.PlayerCode,
            request.TrsId);

        var result = await fundStatusService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            State = result
        };
    }

    public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
    {
        var query = new GameUrlQuery(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.ClientType,
            request.LanguageId,
            request.PlayerCode);

        var result = await gameUrlService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new GameUrlReply
        {
            GameUrl = result
        };
    }

    public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
    {
        var command = new PlayerCreateCommand(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.PlayerCode,
            request.ClientType,
            request.Currency,
            request.LanguageId);

        await playerCreateService
            .ExecuteAsync(command, context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            State = true
        };
    }

    public override async Task<BaseReply> PlayerCreateByBetLimit(PlayerCreateByBetLimitRequest request, ServerCallContext context)
    {
        var command = new PlayerCreateByBetLimitCommand(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.PlayerCode,
            request.ClientType,
            request.Currency,
            request.LanguageId,
            request.Quotas.Select(x => x));

        await playerCreateByBetLimitService
            .ExecuteAsync(command, context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            State = true
        };
    }

    public override async Task<BaseReply> PlayerUpdateBetLimit(PlayerUpdateBetLimitRequest request, ServerCallContext context)
    {
        var command = new PlayerUpdateBetLimitCommand(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.PlayerCode,
            request.Quotas.Select(x => x));

        var result = await playerUpdateBetLimitService
            .GetAsync(command, context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            State = result
        };
    }

    public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
    {
        var command = new PlayerLogoutCommand(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.PlayerCode);

        await playerLogoutService
            .ExecuteAsync(command, context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            State = true
        };
    }

    public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
    {
        var query = new PlayerBalanceQuery(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.PlayerCode);

        var result = await playerBalanceService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceReply
        {
            IsPlayerExist = result.IsPlayerExist,
            Balance = result.Balance,
        };
    }

    public override async Task<GameRecordInfoReply> GameRecordInquire(GameRecordInquireRequest request, ServerCallContext context)
    {
        var query = new GameRecordInquire(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.BeginTime.ToDateTimeOffset(),
            request.EndTime.ToDateTimeOffset(),
            request.PageSize,
            request.PageIndex);

        var results = await gameRecordInquireService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        var reply = new GameRecordInfoReply();

        if (results.Data.Any())
        {
            foreach (var result in results.Data)
            {
                reply.Items.Add(new Protos.GameRecordInfo
                {
                    ExternalRoundId = result.ExternalRoundId,
                    ExternalGameId = result.ExternalGameId,
                    PlayerCode = result.PlayerCode,
                    ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
                    Bet = result.Bet,
                    ValidBet = result.ValidBet,
                    Settle = result.Settle,
                    Turnover = result.Turnover,
                    WinLoss = result.PlayerWinLoss,
                    AdditionalData = result.AdditionalData,
                    IsClosed = result.IsClosed,
                    IsVoid = result.IsVoid,
                });
            }
        }
        reply.ArraySize = results.ArraySize;
        reply.Total = results.Total;

        return reply;
    }

    public override async Task<GameRecordReply> GameRecord(GameRecordRequest request, ServerCallContext context)
    {
        var query = new GameRecordQuery(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.Size);

        var results = await gameRecordService
            .QueryAsync(query, context.CancellationToken)
            .ToArrayAsync(context.CancellationToken)
            .ConfigureAwait(false);

        var reply = new GameRecordReply();

        if (results.Length != 0)
        {
            foreach (var result in results)
            {
                reply.Items.Add(new Protos.GameRecordInfo
                {
                    ExternalRoundId = result.ExternalRoundId,
                    ExternalGameId = result.ExternalGameId,
                    PlayerCode = result.PlayerCode,
                    ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
                    Bet = result.Bet,
                    ValidBet = result.ValidBet,
                    Settle = result.Settle,
                    Turnover = result.Turnover,
                    WinLoss = result.PlayerWinLoss,
                    AdditionalData = result.AdditionalData,
                    IsClosed = result.IsClosed,
                    IsVoid = result.IsVoid,
                });
            }
        }

        return reply;
    }

    public override async Task<BaseReply> GameRecordMark(GameRecordMarkRequest request, ServerCallContext context)
    {
        var query = new GameRecordMarkQuery(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.ExternalRoundIds.ToArray());

        var result = await gameRecordMarkService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            State = result
        };
    }
}
