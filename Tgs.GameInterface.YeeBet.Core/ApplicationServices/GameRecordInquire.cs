namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;
public record GameRecordInquire(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    DateTimeOffset BeginTime,
    DateTimeOffset EndTime,
    int PageSize,
    int PageIndex);
