using System;
using AuthService.Domain.Aggregates.AccountAggregate;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;

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
