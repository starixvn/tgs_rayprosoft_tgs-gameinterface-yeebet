namespace Tgs.GameInterface.YeeBet.ViewModels;

public record GameRecordInquireViewModel(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    DateTimeOffset BeginTime,
    DateTimeOffset EndTime,
    int PageSize,
    int PageIndex);
