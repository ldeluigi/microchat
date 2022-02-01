using System.Text.RegularExpressions;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Values;

namespace AuthService.Domain.Aggregates.AccountAggregate;

public record UsernameEmptyError : DomainError;

public record UsernameLengthError(int MinimumLength) : DomainError;

public record UsernamePatternError : DomainError;

public record Username : ValueWrapper<string, Username>
{
    public const string Pattern = @"^[A-Za-z][A-Za-z0-9]*(_[A-Za-z0-9]{2,}){0,3}$";
    public const int MinimumLenght = 4;

    private Username(string username) : base(username)
    {
        DomainConstraints.Check()
            .If(string.IsNullOrWhiteSpace(username), () => new UsernameEmptyError())
            .If(username.Length < MinimumLenght, () => new UsernameLengthError(MinimumLenght))
            .IfNot(Regex.IsMatch(username, Pattern), () => new UsernamePatternError())
            .ThrowException();
    }

    public static Username From(string username) => new(username);
}
