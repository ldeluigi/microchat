using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Tokens;
using AuthService.Infrastructure.DataAccess.Model.AccountAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.Options;
using System.Linq;

namespace AuthService.Infrastructure.DataAccess.ModelConverters;

public class AccountConverter : IModelConverter<Account, AccountModel>
{
    private readonly SessionConverter _sessionConverter = new();

    public Account ToDomain(AccountModel model)
    {
        var token = model.PasswordRecoveryToken.AsOption()
            .Map(t => new TokenInfo(Token.From(t), model.PasswordRecoveryTokenExpiration));
        var pendingConfirmation = model.ConfirmationToken.AsOption()
            .Map(t => new EmailConfirmationInfo(
                model.EmailUpdate.AsOption().Map(Email.From),
                new TokenInfo(Token.From(model.ConfirmationToken), model.ConfirmationTokenExpiration)));
        var sessions = AccountSessions.Create(model.Sessions.Select(_sessionConverter.ToDomain));

        return new(
            id: model.Id,
            username: Username.From(model.Username),
            creation: model.Creation,
            email: Email.From(model.Email),
            isActive: model.IsActive,
            pendingConfirmation: pendingConfirmation,
            sessions: sessions,
            passwordHash: new PasswordHash(model.Password, model.Salt),
            passwordRecoveryToken: token);
    }

    public void ApplyChanges(Account origin, AccountModel destination)
    {
        destination.Id = origin.Id;
        destination.Username = origin.Username;
        destination.Email = origin.Email;
        destination.IsActive = origin.IsActive;
        (destination.Password, destination.Salt) = origin.PasswordHash;
        (destination.PasswordRecoveryToken, destination.PasswordRecoveryTokenExpiration) = origin.PasswordRecoveryToken.Match(
            some: t => (t.Value.ToString(), t.Expiration),
            none: () => (null, null));
        (destination.ConfirmationToken, destination.ConfirmationTokenExpiration, destination.EmailUpdate) = origin
            .PendingConfirmation
            .Match(
                some: c => (c.Token.Value.ToString(), c.Token.Expiration, c.NewEmail.Map(x => x.ToString()).OrElseNull()),
                none: () => (null, null, null));
        destination.Creation = origin.Creation;
        destination.Sessions.Clear();
        ConversionUtils.ApplyChangesToCollection(
            origin.Sessions,
            s => s.RefreshToken,
            destination.Sessions,
            s => s.RefreshToken,
            _sessionConverter);
    }
}

public class SessionConverter : IModelConverter<Session, SessionModel>
{
    public Session ToDomain(SessionModel model) =>
        new(model.AccessTokenId, Token.From(model.RefreshToken), model.Expiration);

    public void ApplyChanges(Session session, SessionModel model)
    {
        model.AccessTokenId = session.AccessTokenId;
        model.RefreshToken = session.RefreshToken.ToString();
        model.Expiration = session.Expiration;
    }
}
