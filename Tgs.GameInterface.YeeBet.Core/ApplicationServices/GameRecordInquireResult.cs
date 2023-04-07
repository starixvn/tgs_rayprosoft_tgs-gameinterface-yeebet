namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;
public record GameRecordInquireResult
{
    public int Total { get; init; }
    public int ArraySize { get; init; }
    public IEnumerable<GameRecordInfo> Data { get; init; } = Enumerable.Empty<GameRecordInfo>();
}
