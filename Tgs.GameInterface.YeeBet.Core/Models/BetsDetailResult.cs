namespace Tgs.GameInterface.YeeBet.Core.Models;
public record BetsDetailResult
{
    public int Total { get; init; }
    public int ArraySize { get; init; }
    public IEnumerable<BetDetailResult> Data { get; init; } = Enumerable.Empty<BetDetailResult>();
}
