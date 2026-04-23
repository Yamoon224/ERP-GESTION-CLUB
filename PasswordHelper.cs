using System.Security.Cryptography;

namespace guinea_football_club_erp;

/// <summary>
/// Hachage sécurisé des mots de passe via PBKDF2-SHA256 (100 000 itérations).
/// </summary>
public static class PasswordHelper
{
    private const int Iterations = 100_000;
    private const int KeySize    = 32; // 256 bits

    public static (string hash, string salt) Hash(string password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var salt = Convert.ToBase64String(saltBytes);
        var hash = Derive(password, saltBytes);
        return (hash, salt);
    }

    public static bool Verify(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var computed  = Derive(password, saltBytes);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(computed),
            Convert.FromBase64String(hash));
    }

    private static string Derive(string password, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password, salt, Iterations, HashAlgorithmName.SHA256);
        return Convert.ToBase64String(pbkdf2.GetBytes(KeySize));
    }
}
