using AuthService.Domain.Aggregates.AccountAggregate;
using EasyDesk.CleanArchitecture.Domain.Time;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;

namespace AuthService.Domain.Authentication.Accounts;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAccessTokenService _accessTokenService;
    private readonly ITimestampProvider _timestampProvider;
    private readonly Duration _sessionDuration;

    public AuthenticationService(
        IAccessTokenService accessTokenService,
        ITimestampProvider timestampProvider,
        Duration sessionDuration)
    {
        _accessTokenService = accessTokenService;
        _timestampProvider = timestampProvider;
        _sessionDuration = sessionDuration;
    }

    public AuthenticationResult GenerateAuthenticationResult(Account account)
    {
        var accessToken = _accessTokenService.GenerateAccessToken(account);
        var expiration = _timestampProvider.Now + _sessionDuration.AsTimeOffset;
        var session = account.StartNewSession(accessToken.Id, expiration);
        return new AuthenticationResult(
            account.Id,
            accessToken.Value,
            session.RefreshToken,
            accessToken.Expiration);
    }
}
