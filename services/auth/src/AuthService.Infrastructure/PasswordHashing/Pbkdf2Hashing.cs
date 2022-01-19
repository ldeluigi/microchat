using System.Security.Cryptography;

namespace AuthService.Infrastructure.PasswordHashing;

public class Pbkdf2Hashing : IHashCalculator
{
    private const int HashIterations = 9947;

    public byte[] Hash(string password, byte[] salt, int length)
    {
        using var hashCalc = new Rfc2898DeriveBytes(password, salt, HashIterations);
        return hashCalc.GetBytes(length);
    }
}
