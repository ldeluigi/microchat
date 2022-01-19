using System;

namespace AuthService.Infrastructure.Authentication;

public class TokensSettings
{
    public TimeSpan RefreshTokenLifetime { get; set; }

    public TimeSpan PasswordTokenLifetime { get; set; }

    public TimeSpan AccountVerificationTokenLifetime { get; set; }
}
