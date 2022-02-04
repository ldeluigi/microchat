using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Values;

namespace ChatService.Domain.Aggregates.MessageAggregate;

public record EmptyMessageError() : DomainError;

public record MessageTextTooLong(int Length, int MaximumLength) : DomainError;

public record MessageText : ValueWrapper<string, MessageText>
{
    public const int MaximumLength = 4000;

    public MessageText(string value) : base(value)
    {
        DomainConstraints.Check()
            .If(string.IsNullOrWhiteSpace(value), () => new EmptyMessageError())
            .If(value.Length > MaximumLength, () => new MessageTextTooLong(value.Length, MaximumLength))
            .ThrowException();
    }

    protected override string StringRepresentation() => Value;
}
