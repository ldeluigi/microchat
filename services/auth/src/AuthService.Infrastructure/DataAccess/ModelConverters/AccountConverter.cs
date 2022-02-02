using System.Linq;
using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Infrastructure.DataAccess.Model.AccountAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.Options;

namespace AuthService.Infrastructure.DataAccess.ModelConverters;

public class AccountConverter : IModelConverter<Account, AccountModel>
{
    private readonly SessionConverter _sessionConverter = new();

    public Account ToDomain(AccountModel model)
    {
        var sessions = AccountSessions.Create(model.Sessions.Select(_sessionConverter.ToDomain));

        return Account.Create(
            id: model.Id,
            username: Username.From(model.Username),
            creation: model.Creation,
            email: Email.From(model.Email),
            isActive: model.IsActive,
            sessions: sessions,
            passwordHash: new PasswordHash(model.Password, model.Salt));
    }

    public void ApplyChanges(Account origin, AccountModel destination)
    {
        destination.Id = origin.Id;
        destination.Username = origin.Username;
        destination.Email = origin.Email;
        destination.IsActive = origin.IsActive;
        (destination.Password, destination.Salt) = origin.PasswordHash;
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
