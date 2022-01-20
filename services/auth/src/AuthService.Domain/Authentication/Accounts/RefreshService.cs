using System.Linq;
using AuthService.Domain.Aggregates.AccountAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.CleanArchitecture.Domain.Time;
using EasyDesk.CleanArchitecture.Domain.Utils;
using EasyDesk.Tools.Options;

namespace AuthService.Domain.Authentication.Accounts;

public record TokenRefreshFailed : DomainError;

public class RefreshService
{
    private readonly ITimestampProvider _timestampProvider;
    private readonly IAuthenticationService _authenticationService;
    private readonly IAccessTokenService _accessTokenService;

    public RefreshService(
        ITimestampProvider timestampProvider,
        IAuthenticationService authenticationService,
        IAccessTokenService accessTokenService)
    {
        _timestampProvider = timestampProvider;
        _authenticationService = authenticationService;
        _accessTokenService = accessTokenService;
    }

    public Result<AuthenticationResult> Refresh(Account account, Token accessToken, Token refreshToken)
    {
        return account.InvalidateSession(refreshToken)
            .Filter(session => session.Expiration > _timestampProvider.Now)
            .Filter(session => AccessTokenIsValidForSession(accessToken, session))
            .Map(_ => _authenticationService.GenerateAuthenticationResult(account))
            .OrElseError(() => new TokenRefreshFailed());
    }

    private bool AccessTokenIsValidForSession(Token accessToken, Session session)
    {
        return _accessTokenService.ValidateForRefresh(accessToken)
            .Contains(session.AccessTokenId);
    }
}
