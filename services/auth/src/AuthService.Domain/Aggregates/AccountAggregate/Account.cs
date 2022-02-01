using System;
using AuthService.Domain.Aggregates.AccountAggregate.Events;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;

namespace AuthService.Domain.Aggregates.AccountAggregate;

public class Account : AggregateRoot
{
    public Account(
        Guid id,
        Email email,
        Username username,
        Timestamp creation,
        bool isActive,
        AccountSessions sessions,
        PasswordHash passwordHash)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = isActive;
        Username = username;
        Creation = creation;
        Sessions = sessions;
    }

    public Guid Id { get; }

    public Email Email { get; private set; }

    public Username Username { get; private set; }

    public Timestamp Creation { get; private set; }

    public AccountSessions Sessions { get; private set; }

    public bool IsActive { get; private set; }

    public PasswordHash PasswordHash { get; private set; }

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
            sessions: AccountSessions.Empty,
            passwordHash: passwordHash);
    }

    public void UpdateEmail(Email email) => Email = email;

    public void UpdatePassword(PasswordHash passwordHash)
    {
        PasswordHash = passwordHash;
        InvalidateAllSessions();
        EmitEvent(new PasswordChangedEvent(this));
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
