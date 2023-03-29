using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;
using Tgs.GameInterface.YeeBet.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.GrpcServices;

public class GameInterfaceYeeBetGrpcService : GameInterfaceYeeBet.GameInterfaceYeeBetBase
{
    private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
    private readonly IGetService<FundInQuery, FundResult> _fundInService;
    private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
    private readonly IGetService<GameUrlQuery, string> _gameUrlService;
    private readonly ICommandService<PlayerCreateCommand> _playerCreateService;
    private readonly ICommandService<PlayerLogoutCommand> _playerLogoutService;
    private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
    private readonly IQueryService<GameRecordHistoryQuery, GameRecordResult> _gameRecordHistoryService;
    private readonly IGetService<GameHistoryMarkQuery, bool> _gameRecordHistoryMarkService;

    public GameInterfaceYeeBetGrpcService(
        IGetService<FundOutQuery, FundResult> fundOutService,
        IGetService<FundInQuery, FundResult> fundInService,
        IGetService<FundStatusQuery, bool> fundStatusService,
        IGetService<GameUrlQuery, string> gameUrlService,
        ICommandService<PlayerCreateCommand> playerCreateService,
        ICommandService<PlayerLogoutCommand> playerLogoutService,
        IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
        IQueryService<GameRecordHistoryQuery, GameRecordResult> gameRecordHistoryService,
        IGetService<GameHistoryMarkQuery, bool> gameRecordHistoryMarkService)
    {
        _fundOutService = fundOutService;
        _fundInService = fundInService;
        _fundStatusService = fundStatusService;
        _gameUrlService = gameUrlService;
        _playerCreateService = playerCreateService;
        _playerLogoutService = playerLogoutService;
        _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
        _gameRecordHistoryService = gameRecordHistoryService;
        _gameRecordHistoryMarkService = gameRecordHistoryMarkService;
    }

    public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
    {
        var query = new FundOutQuery(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.PlayerCode,
            request.Amount);

        var result = await _fundOutService
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

        var result = await _fundInService
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

        var result = await _fundStatusService
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

        var result = await _gameUrlService
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

        await _playerCreateService
            .ExecuteAsync(command, context.CancellationToken)
            .ConfigureAwait(false);
        return new BaseReply
        {
            State = true
        };
    }

    public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
    {
        var command = new PlayerLogoutCommand(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.PlayerCode);

        await _playerLogoutService
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

        var result = await _playerBalanceService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceReply
        {
            IsPlayerExist = result.IsPlayerExist,
            Balance = result.Balance,
        };
    }

    public override async Task<GameRecordReply> GameRecord(GameRecordRequest request, ServerCallContext context)
    {
        var query = new GameRecordHistoryQuery(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.Size);

        var results = await _gameRecordHistoryService
            .QueryAsync(query, context.CancellationToken)
            .ToArrayAsync(context.CancellationToken)
            .ConfigureAwait(false);

        var reply = new GameRecordReply();

        if (results.Any())
        {
            foreach (var result in results)
            {
                reply.Items.Add(new GameRecordInfo
                {
                    ExternalRoundId = result.ExternalRoundId,
                    ExternalGameId = result.ExternalGameId,
                    ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
                    PlayerCode = result.PlayerCode,
                    Bet = result.Bet,
                    Settle = result.Settle,
                    ValidBet = result.ValidBet,
                    Turnover = result.Turnover,
                    WinLoss = result.PlayerWinLoss,
                    AdditionalData = result.AdditionalData,
                });
            }
        }

        return reply;
    }

    public override async Task<BaseReply> GameRecordMark(GameRecordMarkRequest request, ServerCallContext context)
    {
        var query = new GameHistoryMarkQuery(
            new Uri(request.ApiUrl),
            request.AppId,
            request.SecretKey,
            request.ExternalRoundIds.ToArray());

        var result = await _gameRecordHistoryMarkService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            State = result
        };
    }
}
