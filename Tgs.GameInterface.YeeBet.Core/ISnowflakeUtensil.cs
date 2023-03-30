namespace Tgs.GameInterface.YeeBet.Core;

public interface ISnowflakeUtensil
{
    long GetDatacenterId();

    long GetWorkerId();

    long NextId();
}
