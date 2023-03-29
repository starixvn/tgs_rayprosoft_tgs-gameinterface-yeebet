namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

public record GameRecordHistoryQuery(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    int Size);
