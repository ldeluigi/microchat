using System;
using AuthService.Domain.Authentication;
using AuthService.Domain.Authentication.Passwords;
using AuthService.Infrastructure.Authentication;
using AuthService.Infrastructure.PasswordHashing;
using EasyDesk.CleanArchitecture.Application.Modules;
using EasyDesk.CleanArchitecture.Infrastructure.Configuration;
using EasyDesk.CleanArchitecture.Infrastructure.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuthService.Web.DependencyInjection;

public class JwtAuthorityModule : IAppModule
{
    public static readonly string JwtScopeName = "Global";
    private readonly IConfiguration _configuration;

    public JwtAuthorityModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services, AppDescription app)
    {
        services
            .AddSingleton(_configuration.RequireValue<TokensSettings>("TokensSettings"))
            .AddSingleton(_configuration.ReadJwtSettings(JwtScopeName))
            .AddScoped<IHashingService, HashingService>()
            .AddScoped<ISaltGenerator, RandomSaltGenerator>()
            .AddScoped<IHashCalculator, Pbkdf2Hashing>()
            .AddScoped(CreateAccessTokenService);
    }

    private IAccessTokenService CreateAccessTokenService(IServiceProvider provider)
    {
        return new JwtAccessTokenService(provider.GetRequiredService<JwtService>(), provider.GetRequiredService<JwtSettings>(), provider.GetRequiredService<ILogger<JwtAccessTokenService>>());
    }
}
