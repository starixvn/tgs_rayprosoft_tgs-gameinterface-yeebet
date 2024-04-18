namespace Tgs.GameInterface.YeeBet.Core;

public interface IYeeBetHasher
{
	public string ComputeHash(IDictionary<string, string> dict, string secretKey);
}
