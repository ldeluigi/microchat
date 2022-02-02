using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Values;

namespace UserService.Domain.Aggregates.UserAggregate;

public record NameEmptyError : DomainError;

public record NameLengthError(int MinimumLength, int MaximumLength) : DomainError;

public record NamePatternError : DomainError;

public record Name : ValueWrapper<string, Name>
{
    public const int MinimumLength = 4;
    public const int MaximumLength = 100;

    private Name(string name) : base(name)
    {
        DomainConstraints.Check()
            .If(string.IsNullOrWhiteSpace(name), () => new NameEmptyError())
            .If(name.Length < MinimumLength || name.Length > MaximumLength, () =>
                new NameLengthError(MinimumLength, MaximumLength))
            .ThrowException();
    }

    public static Name From(string name) => new(name);
}
