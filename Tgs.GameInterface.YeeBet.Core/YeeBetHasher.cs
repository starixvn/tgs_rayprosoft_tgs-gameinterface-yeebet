using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Tgs.GameInterface.YeeBet.Core;

public class YeeBetHasher : IYeeBetHasher
{
    private readonly ILogger<YeeBetHasher> _logger;

    public YeeBetHasher(ILogger<YeeBetHasher> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public string ComputeHash(IDictionary<string, string> dict, string secretKey)
    {
        var sortDict = dict
            .OrderBy(x => x.Key)
            .Select(x =>
            {
                return $"{x.Key}={x.Value}";
            });

        var content = string.Join("&", sortDict);

        var plainText = $"{content}&key={secretKey}";

        _logger.LogInformation("YeeBetHasher ComputeHash sortDict:{sortDict}, secretKey:{secretKey}", JsonSerializer.Serialize(sortDict), secretKey);

        var data = Encoding.UTF8.GetBytes(plainText);

        var hash = MD5.HashData(data);

        return Convert.ToHexString(hash).ToLower();
    }
}
