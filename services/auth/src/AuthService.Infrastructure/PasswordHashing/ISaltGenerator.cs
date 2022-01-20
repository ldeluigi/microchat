namespace AuthService.Infrastructure.PasswordHashing;

public interface ISaltGenerator
{
    byte[] NewSalt(int length);
}
