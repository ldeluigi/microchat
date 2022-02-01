using System;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;

namespace AuthService.Domain.Aggregates.AccountAggregate;

public class Session : Entity
{
    public Session(Guid accessTokenId, Token refreshToken, Timestamp expiration)
    {
        RefreshToken = refreshToken;
        Expiration = expiration;
        AccessTokenId = accessTokenId;
    }

    public Token RefreshToken { get; }

    public Timestamp Expiration { get; }

    public Guid AccessTokenId { get; }
}
