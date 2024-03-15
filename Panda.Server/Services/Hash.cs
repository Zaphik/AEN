using System.Security.Cryptography;
using System.Text;

namespace Panda.Server.Models;

// This class is used to hash and compare passwords
// A bit of a long day explaining this one
public static class Hash
{
    private static byte[] salt { get; set; }
    private static readonly string pepper;
    private static readonly byte[] hashBytes;

    static Hash()
    {
        salt = new byte[32];
        pepper = "ICanHackYou";
        hashBytes = new byte[48];
    }

    public static string GetHash(string password)
    {
        var semiseasonedPassword = pepper + password;
        var sha384Bytes = SHA384.HashData(Encoding.UTF8.GetBytes(semiseasonedPassword));
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var pbkdf2 = new Rfc2898DeriveBytes(sha384Bytes, salt, 10000, HashAlgorithmName.SHA384);
        var derivedKey = pbkdf2.GetBytes(hashBytes.Length);

        var seasonedPassword = new byte[43 + derivedKey.Length];
        salt.CopyTo(seasonedPassword, 0);
        Encoding.UTF8.GetBytes(pepper).CopyTo(seasonedPassword, 32);
        derivedKey.CopyTo(seasonedPassword, 43);

        return Convert.ToBase64String(seasonedPassword);
    }

    public static bool CompareHash(string password, string savedPasswordHash)
    {
        var seasonedPassword = Convert.FromBase64String(savedPasswordHash);
        salt = seasonedPassword[..32];

        var semiseasonedPassword = pepper + password;
        var sha384Bytes = SHA384.HashData(Encoding.UTF8.GetBytes(semiseasonedPassword));
        var pbkdf2 = new Rfc2898DeriveBytes(sha384Bytes, salt, 10000, HashAlgorithmName.SHA384);
        var derivedKey = pbkdf2.GetBytes(hashBytes.Length);

        var calculatedPasswordHash = new byte[43 + derivedKey.Length];
        salt.CopyTo(calculatedPasswordHash, 0);
        Encoding.UTF8.GetBytes(pepper).CopyTo(calculatedPasswordHash, 32);
        derivedKey.CopyTo(calculatedPasswordHash, 43);

        return Convert.ToBase64String(calculatedPasswordHash) == savedPasswordHash;
    }
}