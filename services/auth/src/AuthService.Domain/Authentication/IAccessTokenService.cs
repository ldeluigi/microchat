using System;
using AuthService.Domain.Aggregates.AccountAggregate;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;

namespace AuthService.Domain.Authentication;

public interface IAccessTokenService
{
    AccessToken GenerateAccessToken(Account account);

    Option<Guid> ValidateForRefresh(Token accessToken);
}

public record AccessToken(Guid Id, Token Value, Timestamp Expiration);
