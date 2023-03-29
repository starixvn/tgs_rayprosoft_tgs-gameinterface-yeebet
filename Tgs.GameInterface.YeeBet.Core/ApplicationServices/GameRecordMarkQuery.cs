namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

public record GameRecordMarkQuery(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    IEnumerable<string> ExternalRoundIds);
