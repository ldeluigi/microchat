using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace AuthService.Domain.Tokens;

public record InvalidToken : DomainError;

public record TokenInfo(Token Value, Timestamp Expiration)
{
    public static TokenInfo Random(Timestamp expiration) => new(Token.Random(), expiration);

    public Result<Nothing> Validate(Token valueToValidate, Timestamp now)
    {
        return RequireTrue(
            valueToValidate == Value && Expiration > now,
            () => new InvalidToken());
    }
}
