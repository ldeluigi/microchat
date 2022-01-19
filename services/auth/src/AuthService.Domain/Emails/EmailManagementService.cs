using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Tokens;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.CleanArchitecture.Domain.Time;
using EasyDesk.CleanArchitecture.Domain.Utils;
using EasyDesk.Tools;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using static EasyDesk.Tools.Options.OptionImports;

namespace AuthService.Domain.Emails;

public record NoPendingConfirmation : DomainError;

public class EmailManagementService
{
    private readonly ITimestampProvider _timestampProvider;
    private readonly Duration _tokenDuration;

    public EmailManagementService(
        ITimestampProvider timestampProvider,
        Duration tokenDuration)
    {
        _timestampProvider = timestampProvider;
        _tokenDuration = tokenDuration;
    }

    public Result<Nothing> RegenerateEmailConfirmationToken(Account account)
    {
        return RequirePendingConfirmation(account)
            .IfSuccess(c => AssignNewConfirmationToken(account, c.NewEmail));
    }

    public void GenerateEmailConfirmationTokenForRegisteredUser(Account account)
    {
        AssignNewConfirmationToken(account, None);
    }

    private void AssignNewConfirmationToken(Account account, Option<Email> newEmail)
    {
        var token = NewEmailConfirmationToken();
        account.StartNewEmailConfirmation(new EmailConfirmationInfo(newEmail, token));
    }

    private TokenInfo NewEmailConfirmationToken() => TokenInfo.Random(_timestampProvider.Now + _tokenDuration.AsTimeOffset);

    public Result<Nothing> ConfirmEmail(Account account, Token tokenToValidate)
    {
        return RequirePendingConfirmation(account)
            .Require(c => c.Token.Validate(tokenToValidate, _timestampProvider.Now))
            .IfSuccess(c => account.ConfirmEmail());
    }

    private static Result<EmailConfirmationInfo> RequirePendingConfirmation(Account account)
    {
        return account
            .PendingConfirmation
            .OrElseError(() => new NoPendingConfirmation());
    }
}
