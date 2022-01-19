namespace AuthService.Infrastructure.PasswordHashing;

public interface IHashCalculator
{
    byte[] Hash(string password, byte[] salt, int length);
}
