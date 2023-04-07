namespace Tgs.GameInterface.YeeBet.ViewModels;

public class GameRecordInquireResultViewModel
{
    public int Total { get; set; }

    public int ArraySize { get; set; }

    public IEnumerable<GameRecordInfoViewModel> Data { get; set; } = Enumerable.Empty<GameRecordInfoViewModel>();
}
