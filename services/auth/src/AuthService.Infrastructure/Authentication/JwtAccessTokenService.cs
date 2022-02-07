using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.CleanArchitecture.Infrastructure.Jwt;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using Microsoft.Extensions.Logging;
using static EasyDesk.Tools.Options.OptionImports;

namespace AuthService.Infrastructure.Authentication;

public class JwtAccessTokenService : IAccessTokenService
{
    private readonly JwtFacade _jwtService;
    private readonly JwtTokenConfiguration _jwtTokenConfiguration;
    private readonly JwtValidationConfiguration _jwtValidationConfiguration;
    private readonly ILogger<JwtAccessTokenService> _logger;

    public JwtAccessTokenService(
        JwtFacade jwtService,
        JwtTokenConfiguration jwtTokenConfiguration,
        JwtValidationConfiguration jwtValidationConfiguration,
        ILogger<JwtAccessTokenService> logger)
    {
        _jwtService = jwtService;
        _jwtTokenConfiguration = jwtTokenConfiguration;
        _jwtValidationConfiguration = jwtValidationConfiguration;
        _logger = logger;
    }

    public AccessToken GenerateAccessToken(Account account)
    {
        var id = Guid.NewGuid();
        var idClaim = new Claim(JwtClaimNames.JwtId, id.ToString());
        var tokenString = _jwtService.Create(
            CreateClaimsList(account).Append(idClaim),
            out var token,
            _jwtTokenConfiguration);
        return new(id, Token.From(tokenString), Timestamp.FromUtcDateTime(token.ValidTo));
    }

    private IEnumerable<Claim> CreateClaimsList(Account user)
    {
        return new List<Claim>() { new Claim(JwtClaimNames.Subject, user.Id.ToString()) };
    }

    public Option<Guid> ValidateForRefresh(Token accessToken)
    {
        return from claimsIdentity in ValidateAccessTokenWithoutLifetime(accessToken)
               from claim in claimsIdentity.FindFirst(JwtClaimNames.JwtId).AsOption()
               from parsedId in FromTryConstruct<string, Guid>(claim.Value, Guid.TryParse)
               select parsedId;
    }

    private Option<ClaimsPrincipal> ValidateAccessTokenWithoutLifetime(Token accessToken)
    {
        return _jwtService.Validate(accessToken.ToString(), configure =>
            _jwtValidationConfiguration(configure)
            .WithoutLifetimeValidation());
    }
}
