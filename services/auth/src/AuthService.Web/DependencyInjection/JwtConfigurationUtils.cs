using EasyDesk.CleanArchitecture.Domain.Time;
using EasyDesk.CleanArchitecture.Infrastructure.Configuration;
using EasyDesk.CleanArchitecture.Infrastructure.Jwt;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;

namespace AuthService.Web.DependencyInjection;

public static class JwtConfigurationUtils
{
    public static readonly string JwtConfigurationKey = "JwtScopes";
    public static readonly string JwtScopeName = "Global";

    public static JwtValidationConfiguration GetJwtValidationConfiguration(
        IConfiguration configuration,
        string appName) =>
        configure =>
            configure
                .WithSigningCredentials(ReadKeyFromConfiguration(configuration))
                .WithIssuerValidation(appName)
                .WithAudienceValidation(GetAudienceName(configuration));

    public static JwtTokenConfiguration GetJwtTokenConfiguration(
        IConfiguration configuration,
        string appName,
        ITimestampProvider timestampProvider) =>
        configure =>
            configure
                .WithSigningCredentials(
                    ReadKeyFromConfiguration(configuration),
                    SecurityAlgorithms.HmacSha256Signature)
                .WithLifetime(
                    Duration.FromTimeSpan(JwtConfiguration(configuration).RequireValue<TimeSpan>("Lifetime")),
                    timestampProvider)
                .WithIssuer(appName)
                .WithAudience(GetAudienceName(configuration));

    private static string GetAudienceName(IConfiguration configuration) =>
        JwtConfiguration(configuration).GetValue<string>("Audience");

    private static IConfigurationSection JwtConfiguration(IConfiguration configuration) =>
        configuration.RequireSection(JwtConfigurationKey).RequireSection(JwtScopeName);

    private static SecurityKey ReadKeyFromConfiguration(IConfiguration configuration)
        => KeyUtils.KeyFromString(JwtConfiguration(configuration).RequireValue<string>("Key"));
}
