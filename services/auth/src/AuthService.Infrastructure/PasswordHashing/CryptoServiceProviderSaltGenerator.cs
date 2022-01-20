using System.Security.Cryptography;

namespace AuthService.Infrastructure.PasswordHashing;

public sealed class RandomSaltGenerator : ISaltGenerator
{
    public byte[] NewSalt(int length)
    {
        var salt = new byte[length];
        RandomNumberGenerator.Fill(salt);
        return salt;
    }
}
