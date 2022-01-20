using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Values;
using System.Text.RegularExpressions;

namespace UserService.Domain.Aggregates.UserAggregate;

public record NameEmptyError : DomainError;

public record NameLengthError(int MinimumLength) : DomainError;

public record NamePatternError : DomainError;

public record Name : ValueWrapper<string, Name>
{
    private const string Pattern = @"^[A-Za-z]+( [A-Za-z]+)*";
    private const int MinimumLength = 4;
    private const int MaximumLength = 25;

    private Name(string name) : base(name)
    {
        DomainConstraints.Check()
            .If(string.IsNullOrWhiteSpace(name), () => new NameEmptyError())
            .If(name.Length < MinimumLength, () => new NameLengthError(MinimumLength))
            .If(name.Length > MaximumLength, () => new NameLengthError(MaximumLength))
            .IfNot(Regex.IsMatch(name, Pattern), () => new NamePatternError())
            .ThrowException();
    }

    public static Name From(string name) => new(name);
}
