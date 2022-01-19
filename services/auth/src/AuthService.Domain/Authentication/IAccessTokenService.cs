using AuthService.Domain.Aggregates.AccountAggregate;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace AuthService.Domain.Authentication;

public interface IAccessTokenService
{
    AccessToken GenerateAccessToken(Account account);

    Option<Guid> ValidateForRefresh(Token accessToken);
}

public record AccessToken(Guid Id, Token Value, Timestamp Expiration);
