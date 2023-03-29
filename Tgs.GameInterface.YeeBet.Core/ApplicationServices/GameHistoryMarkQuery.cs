namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

public record GameHistoryMarkQuery(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    IEnumerable<string> ExternalRoundIds);
