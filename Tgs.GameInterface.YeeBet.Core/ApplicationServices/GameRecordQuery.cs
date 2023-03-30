namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

public record GameRecordQuery(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    int Size);
