using AuthService.Domain.Aggregates.AccountAggregate.Events;
using AuthService.Domain.Tokens;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;
using static EasyDesk.Tools.Options.OptionImports;

namespace AuthService.Domain.Aggregates.AccountAggregate;

public class Account : AggregateRoot
{
    public Account(
        Guid id,
        Email email,
        Username username,
        Timestamp creation,
        bool isActive,
        Option<EmailConfirmationInfo> pendingConfirmation,
        AccountSessions sessions,
        PasswordHash passwordHash,
        Option<TokenInfo> passwordRecoveryToken)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = isActive;
        PasswordRecoveryToken = passwordRecoveryToken;
        Username = username;
        Creation = creation;
        PendingConfirmation = pendingConfirmation;
        Sessions = sessions;
    }

    public Guid Id { get; }

    public Email Email { get; private set; }

    public Username Username { get; private set; }

    public Timestamp Creation { get; private set; }

    public AccountSessions Sessions { get; private set; }

    public Option<EmailConfirmationInfo> PendingConfirmation { get; private set; }

    public bool IsActive { get; private set; }

    public PasswordHash PasswordHash { get; private set; }

    public Option<TokenInfo> PasswordRecoveryToken { get; private set; }

    public static Account Create(
        Email email,
        PasswordHash passwordHash,
        Username username,
        Timestamp creation)
    {
        return new Account(
            id: Guid.NewGuid(),
            email: email,
            username: username,
            creation: creation,
            isActive: false,
            pendingConfirmation: None,
            sessions: AccountSessions.Empty,
            passwordHash: passwordHash,
            passwordRecoveryToken: None);
    }

    public void UpdateEmail(Email email) => Email = email;

    public void UpdatePassword(PasswordHash passwordHash)
    {
        PasswordHash = passwordHash;
        PasswordRecoveryToken = None;
        InvalidateAllSessions();
        EmitEvent(new PasswordChangedEvent(this));
    }

    public void StartPasswordRecovery(TokenInfo token)
    {
        PasswordRecoveryToken = Some(token);
        EmitEvent(new PasswordRecoveryRequestedEvent(this, token));
    }

    public void ConfirmEmail()
    {
        IsActive = true;
        PendingConfirmation.FlatMap(ci => ci.NewEmail).IfPresent(email =>
        {
            var oldEmail = Email;
            Email = email;
            EmitEvent(new EmailChangedEvent(this, oldEmail));
        });
        PendingConfirmation = None;
    }

    public void StartNewEmailConfirmation(EmailConfirmationInfo confirmationInfo)
    {
        PendingConfirmation = Some(confirmationInfo);
        EmitEvent(new EmailConfirmationRequiredEvent(this, confirmationInfo.Token));
    }

    public Session StartNewSession(Guid accessTokenId, Timestamp expiration)
    {
        var session = new Session(accessTokenId, Token.Random(), expiration);
        Sessions = Sessions.Add(session);
        return session;
    }

    public Option<Session> InvalidateSession(Token refreshToken)
    {
        Sessions = Sessions.InvalidateSession(refreshToken, out var invalidatedSession);
        return invalidatedSession;
    }

    private void InvalidateAllSessions() => Sessions = Sessions.InvalidateAll();
}
