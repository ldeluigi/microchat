using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication.Passwords;
using System;
using System.Linq;

namespace AuthService.Infrastructure.PasswordHashing;

public class HashingService : IHashingService
{
    private const int SaltLength = 128;
    private const int HashLength = 128;

    private readonly IHashCalculator _hashCalculator;
    private readonly ISaltGenerator _saltGenerator;

    public HashingService(IHashCalculator hashCalculator, ISaltGenerator saltGenerator)
    {
        _hashCalculator = hashCalculator;
        _saltGenerator = saltGenerator;
    }

    public PasswordHash GenerateHash(PlainTextPassword password)
    {
        var salt = _saltGenerator.NewSalt(SaltLength);
        var hash = _hashCalculator.Hash(password.ToString(), salt, HashLength);
        return new(Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public bool IsCorrectPassword(PlainTextPassword password, PasswordHash passwordHash)
    {
        var (hash, salt) = passwordHash;
        var computedHash = _hashCalculator.Hash(password, Convert.FromBase64String(salt), HashLength);
        return computedHash.SequenceEqual(Convert.FromBase64String(hash));
    }
}
