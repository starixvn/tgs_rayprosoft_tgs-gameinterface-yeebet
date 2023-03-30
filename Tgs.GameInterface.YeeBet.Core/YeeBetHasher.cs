using System.Security.Cryptography;
using System.Text;

namespace Tgs.GameInterface.YeeBet.Core;

public class YeeBetHasher
{
    private static readonly MD5 _Md5;

    static YeeBetHasher()
    {
        _Md5 = MD5.Create();
    }

    public static string ComputeHash(IDictionary<string, string> dict, string secretKey)
    {
        var sortDict = dict
            .OrderBy(x => x.Key)
            .Select(x =>
            {
                return $"{x.Key}={x.Value}";
            });

        var content = string.Join("&", sortDict);

        var plainText = $"{content}&key={secretKey}";

        var data = Encoding.UTF8.GetBytes(plainText);

        var hash = _Md5.ComputeHash(data);

        return Convert.ToHexString(hash).ToLower();
    }
}
