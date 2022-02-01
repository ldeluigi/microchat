using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.Collections;
using EasyDesk.Tools.Options;
using static EasyDesk.Tools.Collections.ImmutableCollections;

namespace AuthService.Domain.Aggregates.AccountAggregate;

public record AccountSessions : IEnumerable<Session>
{
    private readonly IImmutableSet<Session> _sessions;

    public AccountSessions(IImmutableSet<Session> sessions)
    {
        _sessions = sessions;
    }

    public static AccountSessions Empty { get; } = Create(Enumerable.Empty<Session>());

    public static AccountSessions Create(IEnumerable<Session> sessions) => new(Set(sessions));

    public static AccountSessions Create(params Session[] sessions) => Create(sessions.AsEnumerable());

    public AccountSessions Add(Session session) => new(_sessions.Add(session));

    public AccountSessions InvalidateSession(Token refreshToken, out Option<Session> session)
    {
        session = FindByRefreshToken(refreshToken);
        return session.Match(
            some: s => new AccountSessions(_sessions.Remove(s)),
            none: () => this);
    }

    public AccountSessions InvalidateAll() => new(_sessions.Clear());

    public Option<Session> FindByRefreshToken(Token refreshToken) => _sessions.SingleOption(s => s.RefreshToken == refreshToken);

    public Option<Session> FindByAccessTokenId(Guid id) => _sessions.SingleOption(s => s.AccessTokenId == id);

    public IEnumerator<Session> GetEnumerator() => _sessions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
