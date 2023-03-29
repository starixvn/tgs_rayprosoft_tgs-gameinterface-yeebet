namespace Tgs.GameInterface.YeeBet.ViewModels;

public record GameRecordViewModel(
    Uri ApiUrl,
    string AppId,
    string SecretKey,
    int Size);
