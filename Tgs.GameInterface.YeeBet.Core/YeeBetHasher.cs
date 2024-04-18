using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Tgs.GameInterface.YeeBet.Core;

public class YeeBetHasher(ILogger<YeeBetHasher> logger) : IYeeBetHasher
{
	public string ComputeHash(IDictionary<string, string> dict, string secretKey)
	{
		var sortDict = dict
			.OrderBy(x => x.Key)
			.Select(x => $"{x.Key}={x.Value}");

		var content = string.Join("&", sortDict);

		var plainText = $"{content}&key={secretKey}";

		logger.LogInformation("YeeBetHasher ComputeHash sortDict:{sortDict}, secretKey:{secretKey}", JsonSerializer.Serialize(sortDict), secretKey);

		var data = Encoding.UTF8.GetBytes(plainText);

		var hash = MD5.HashData(data);

		return Convert.ToHexString(hash).ToLower();
	}
}
