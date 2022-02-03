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
    private readonly JwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<JwtAccessTokenService> _logger;

    public JwtAccessTokenService(JwtService jwtService, JwtSettings jwtSettings, ILogger<JwtAccessTokenService> logger)
    {
        _jwtService = jwtService;
        _jwtSettings = jwtSettings;
        _logger = logger;
    }

    public AccessToken GenerateAccessToken(Account account)
    {
        var id = Guid.NewGuid();
        var idClaim = new Claim(JwtClaimNames.JwtId, id.ToString());
        var claimsIdentity = new ClaimsIdentity(CreateClaimsList(account).Append(idClaim));
        try
        {
            var tokenString = _jwtService.CreateToken(claimsIdentity, _jwtSettings, out var token);
            return new(id, Token.From(tokenString), Timestamp.FromUtcDateTime(token.ValidTo));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
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

    private Option<ClaimsIdentity> ValidateAccessTokenWithoutLifetime(Token accessToken)
    {
        return _jwtService.Validate(accessToken.ToString(), _jwtSettings, out var _, validateLifetime: false);
    }
}
