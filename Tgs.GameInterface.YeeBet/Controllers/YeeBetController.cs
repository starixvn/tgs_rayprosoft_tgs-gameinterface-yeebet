using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.YeeBet.Core.ApplicationServices;
using Tgs.GameInterface.YeeBet.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.YeeBet.Controllers;

[Route("yeebet")]
[ApiController]
public class YeeBetController : ControllerBase
{
    /// <summary>
    /// Funds the out asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "playerCode": "4ucmqplxb029",
    ///             "orderId": 1704777287586
    ///             "amount": 100
    ///         }
    /// </remarks>
    [HttpPost("fund/out", Name = "FundOut")]
    public async ValueTask<FundResultViewModel> FundOutAsync(
        [FromServices] IGetService<FundOutQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundOutQuery(
            ApiUrl: source.ApiUrl,
            AppId: source.AppId,
            SecretKey: source.SecretKey,
            PlayerCode: source.PlayerCode,
            OrderId: source.OrderId,
            Amount: source.Amount);

        var result = await service
            .GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new FundResultViewModel(
            TransactionId: result.TransactionId,
            Balance: result.Balance,
            Res: result.Res);
    }

    /// <summary>
    /// Funds the in asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "playerCode": "4ucmqplxb029",
    ///             "orderId": 1704777298592
    ///             "amount": 100
    ///         }
    /// </remarks>
    [HttpPost("fund/in", Name = "FundIn")]
    public async ValueTask<FundResultViewModel> FundInAsync(
        [FromServices] IGetService<FundInQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundInQuery(
            ApiUrl: source.ApiUrl,
            AppId: source.AppId,
            SecretKey: source.SecretKey,
            PlayerCode: source.PlayerCode,
            OrderId: source.OrderId,
            Amount: source.Amount);

        var result = await service
            .GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new FundResultViewModel(
            TransactionId: result.TransactionId,
            Balance: result.Balance,
            Res: result.Res);
    }

    /// <summary>
    /// Games the URL asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "playerCode": "4ucmqplxb029",
    ///             "clientType": 1,
    ///             "languageId": 1
    ///         }
    /// </remarks>
    [HttpPost("gameurl")]
    public ValueTask<string> GameUrlAsync(
        [FromServices] IGetService<GameUrlQuery, string> service,
        GameUrlQueryViewModel source)
    {
        var query = new GameUrlQuery(
            ApiUrl: source.ApiUrl,
            AppId: source.AppId,
            SecretKey: source.SecretKey,
            PlayerCode: source.PlayerCode,
            ClientType: source.ClientType,
            LanguageId: source.LanguageId);

        return service.GetAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "playerCode": "4ucmqplxb029",
    ///             "clientType": 1,
    ///             "currency": "CNY",
    ///             "languageId": 1
    ///         }
    /// </remarks>
    [HttpPost("player/create", Name = "PlayerCreate")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: source.ApiUrl,
            AppId: source.AppId,
            SecretKey: source.SecretKey,
            PlayerCode: source.PlayerCode,
            ClientType: source.ClientType,
            Currency: source.Currency,
            LanguageId: source.LanguageId);

        return service.ExecuteAsync(
            command: command,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "playerCode": "4ucmqplxb029",
    ///             "clientType": 1,
    ///             "currency": "CNY",
    ///             "languageId": 1,
    ///             "quotas": []
    ///         }
    /// </remarks>
    [HttpPost("player/create/betlimit", Name = "PlayerCreateByBetLimit")]
    public ValueTask PlayerCreateByBetLimitAsync(
        [FromServices] ICommandService<PlayerCreateByBetLimitCommand> service,
        PlayerCreateByBetLimitViewModel source)
    {
        var command = new PlayerCreateByBetLimitCommand(
            ApiUrl: source.ApiUrl,
            AppId: source.AppId,
            SecretKey: source.SecretKey,
            PlayerCode: source.PlayerCode,
            ClientType: source.ClientType,
            Currency: source.Currency,
            LanguageId: source.LanguageId,
            Quotas: source.Quotas);

        return service.ExecuteAsync(
            command: command,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "playerCode": "4ucmqplxb029",
    ///             "quotas": [5,6,7]
    ///         }
    /// </remarks>
    [HttpPost("player/update/betlimit", Name = "PlayerUpdateBetLimit")]
    public ValueTask<bool> PlayerUpdateBetLimitAsync(
        [FromServices] IGetService<PlayerUpdateBetLimitCommand, bool> service,
        PlayerUpdateBetLimitViewModel source)
    {
        var command = new PlayerUpdateBetLimitCommand(
            ApiUrl: source.ApiUrl,
            AppId: source.AppId,
            SecretKey: source.SecretKey,
            PlayerCode: source.PlayerCode,
            Quotas: source.Quotas);

        return service.GetAsync(
            query: command,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "playerCode": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/balance", Name = "PlayerBalance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
        [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
        PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: source.ApiUrl,
            AppId: source.AppId,
            SecretKey: source.SecretKey,
            Username: source.PlayerCode);

        var result = await service
            .GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            result.IsPlayerExist,
            result.Balance);
    }

    /// <summary>
    /// Players the kick asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "playerCode": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/logout", Name = "PlayerLogout")]
    public ValueTask PlayerKickAsync(
        [FromServices] ICommandService<PlayerLogoutCommand> service,
        PlayerLogoutViewModel source)
        => service
            .ExecuteAsync(
                command: new PlayerLogoutCommand(
                    ApiUrl: source.ApiUrl,
                    AppId: source.AppId,
                    SecretKey: source.SecretKey,
                    PlayerCode: source.PlayerCode),
                cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Games the record history asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "size": 1000
    ///         }
    /// </remarks>
    [HttpPost("game/record/history/query", Name = "GameRecord")]
    public async IAsyncEnumerable<GameRecordResultViewModel> GameRecordAsync(
        [FromServices] IQueryService<GameRecordQuery, GameRecordResult> service,
        GameRecordViewModel source)
    {
        var query = new GameRecordQuery(
            ApiUrl: source.ApiUrl,
            AppId: source.AppId,
            SecretKey: source.SecretKey,
            Size: source.Size);

        var results = await service
            .QueryAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ToArrayAsync(
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new GameRecordResultViewModel(
                ExternalRoundId: result.ExternalRoundId,
                ExternalGameId: result.ExternalGameId,
                PlayerCode: result.PlayerCode,
                ExternalCreatedOn: result.ExternalCreatedOn,
                ExternalClosedOn: result.ExternalClosedOn,
                Bet: result.Bet,
                ValidBet: result.ValidBet,
                Settle: result.Settle,
                Turnover: result.Turnover,
                WinLoss: result.PlayerWinLoss,
                AdditionalData: result.AdditionalData,
                IsClosed: result.IsClosed,
                IsVoid: result.IsVoid);
        }
    }

    /// <summary>
    /// Games the history mark asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "externalIds": [""]
    ///         }
    /// </remarks>
    [HttpPost("game/history/mark", Name = "GameRecordMark")]
    public ValueTask<bool> GameRecordMarkAsync(
        [FromServices] IGetService<GameRecordMarkQuery, bool> service,
        GameHistoryMarkViewModel source)
        => service.GetAsync(
            query: new GameRecordMarkQuery(
                source.ApiUrl,
                source.AppId,
                source.SecretKey,
                source.ExternalRoundIds),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Funds the status asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "playerCode": "4ucmqplxb029",
    ///             "orderId": 1704777298592
    ///         }
    /// </remarks>
    [HttpPost("fund/Status", Name = "FundStatus")]
    public ValueTask<bool> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, bool> service,
        FundStatusViewModel source)
    {
        var query = new FundStatusQuery(
            ApiUrl: source.ApiUrl,
            AppId: source.AppId,
            SecretKey: source.SecretKey,
            PlayerCode: source.PlayerCode,
            OrderId: source.OrderId);

        return service.GetAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Betses the detail asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://api.98kbus.top/",
    ///             "appId": "xtdA15OTVGEP",
    ///             "secretKey": "4885366DF30D880224FF6AA46FBDE4F0",
    ///             "beginTime": "2023-04-06T08:20:02.868Z",
    ///             "endTime": "2023-04-06T08:20:02.868Z",
    ///             "pageSize": 1000,
    ///             "pageIndex": 0
    ///         }
    /// </remarks>
    [HttpPost("game/record/history/inquire", Name = "GameRecordInquire")]
    public async ValueTask<GameRecordInquireResultViewModel> GameRecordInquireAsync(
        [FromServices] IGetService<GameRecordInquire, GameRecordInquireResult> service,
        GameRecordInquireViewModel source)
    {
        var query = new GameRecordInquire(
            ApiUrl: source.ApiUrl,
            AppId: source.AppId,
            SecretKey: source.SecretKey,
            BeginTime: source.BeginTime,
            EndTime: source.EndTime,
            PageSize: source.PageSize,
            PageIndex: source.PageIndex);

        var results = await service
            .GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new GameRecordInquireResultViewModel
        {
            Total = results.Total,
            ArraySize = results.ArraySize,
            Data = results.Data.Select(result => new GameRecordInfoViewModel(
                ExternalRoundId: result.ExternalRoundId,
                ExternalGameId: result.ExternalGameId,
                PlayerCode: result.PlayerCode,
                ExternalCreatedOn: result.ExternalCreatedOn,
                ExternalClosedOn: result.ExternalClosedOn,
                Bet: result.Bet,
                ValidBet: result.ValidBet,
                Settle: result.Settle,
                Turnover: result.Turnover,
                WinLoss: result.PlayerWinLoss,
                AdditionalData: result.AdditionalData,
                IsClosed: result.IsClosed,
                IsVoid: result.IsVoid))
        };
    }
}
