using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Values;
using System;

namespace AuthService.Domain.Authentication.Passwords;

public class PasswordEmptyError : Exception
{
    public PasswordEmptyError() : base("A password can't be empty.")
    {
    }
}

public class PasswordTooShortError : Exception
{
    public PasswordTooShortError(int actualLength, int minimumLength)
        : base($"A password must be at least {minimumLength} characters long. {actualLength} is not enough.")
    {
    }
}

public record PlainTextPassword : ValueWrapper<string, PlainTextPassword>
{
    public const int MinimumLength = 8;

    private PlainTextPassword(string password) : base(password)
    {
        DomainConstraints.Check()
            .If(string.IsNullOrWhiteSpace(password), () => throw new PasswordEmptyError())
            .If(password.Length < MinimumLength, () => throw new PasswordTooShortError(password.Length, MinimumLength))
            .ThrowException();
    }

    public static PlainTextPassword From(string password) => new(password);
}
