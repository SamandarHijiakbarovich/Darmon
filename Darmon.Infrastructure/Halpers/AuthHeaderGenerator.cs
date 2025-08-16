using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Halpers;

public static class AuthHeaderGenerator
{
    public static string Generate(string merchantUserId, string secretKey)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var digest = GenerateSha1($"{timestamp}{secretKey}");
        return $"{merchantUserId}:{digest}:{timestamp}";
    }

    private static string GenerateSha1(string input)
    {
        using var sha1 = SHA1.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha1.ComputeHash(bytes);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
