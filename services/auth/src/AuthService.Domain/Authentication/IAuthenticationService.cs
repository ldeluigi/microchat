using AuthService.Domain.Aggregates.AccountAggregate;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace AuthService.Domain.Authentication;

public interface IAuthenticationService
{
    AuthenticationResult GenerateAuthenticationResult(Account account);
}

public record AuthenticationResult(
    Guid UserId,
    Token AccessToken,
    Token RefreshToken,
    Timestamp Expiration);
