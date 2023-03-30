namespace Tgs.GameInterface.YeeBet.ViewModels;

public record GameHistoryMarkViewModel(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    IEnumerable<string> ExternalRoundIds);
